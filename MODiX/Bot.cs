using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text.Json;
using Guilded;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using Guilded.Events;
using Guilded.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MODiX.Commands.Commands;
using MODiX.Data;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.Features.Welcomer;
using MODiX.Services.Interfaces;
using MODiX.Services.Services;
using Websocket.Client;

namespace MODiX
{
    public class Bot
    {
        private static readonly string? json   = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json"));
        private static readonly string? token  = JsonSerializer.Deserialize<ConfigJson>(json!)!.Token!;
        private static readonly string? prefix = JsonSerializer.Deserialize<ConfigJson>(json!)!.Prefix!;
        private static readonly string? timePattern = "hh:mm:ss tt";
        private IMessageHandler? msgHandler { get; set; }
        private readonly ModixDbContextFactory? _dbFactory = new();
        public async Task RunAsync()
        {
            msgHandler = new MessageHandler();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            await using var client = new GuildedBotClient(token!)
                .AddCommands(new ModCommands(), prefix!)
                .AddCommands(new MemberCommands(), prefix!)
                .AddCommands(new ConfigCommands(), prefix!)
                .AddCommands(new WikiCommands(), prefix!)
                .AddCommands(new BlackjackCommands(), prefix!)
                .AddCommands(new EconomyCommands(), prefix!);

            client.Prepared
                .Subscribe(async me =>
                {
                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    Console.WriteLine($"[{date}][{time}][INFO]  [{me.ParentClient.Name}] talking to gateway...");
                });

            client.MemberJoined
                .Subscribe(async memJoined =>
                {
                    using var welcomerService = new WelcomerProviderService();
                    var welcomeMsg = await welcomerService.GetRandomWelcomeMessageAsync();
                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    var serverId = memJoined.ServerId;
                    var server = await memJoined.ParentClient.GetServerAsync((HashId)serverId);
                    var defaultChannelId = (Guid)server.DefaultChannelId!;
                    var channel = $"[#📃| rules](https://www.guilded.gg/teams/jynyD3AR/channels/ccefeed6-ab00-4258-836c-14d4cfa3050d/chat)";
                    await memJoined.ParentClient.AddMemberRoleAsync((HashId)serverId, memJoined.Member.Id, 36453250);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"[{date}][{time}][INFO]  [{memJoined.ParentClient.Name}] {memJoined.Name} joined the server.");
                    var newMsg = welcomeMsg!.Message!.Replace("[member]", memJoined.Name).Replace("[server]", server.Name);
                    var embed = new Embed();
                    embed.SetDescription(
                        $"{newMsg} , please read our code of conduct here {channel}");
                    embed.SetFooter("MODiX watching everything ");
                    embed.SetTimestamp(DateTime.Now);
                    await memJoined.ParentClient.CreateMessageAsync(defaultChannelId, true, false, embed);
                    using var memService = new ServerMemberService();
                    var _ = await memService.AddServerMemberToDBAsync(memJoined.ParentClient, memJoined.Member);

                });


            client.Disconnected
                .Where(e => e.Type != DisconnectionType.NoMessageReceived)
                .Subscribe(async me =>
                {
                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"[{date}][{time}][ERROR] [{client.Name}] disconnected from gateway...");
                });

            client.Reconnected
                .Where(x => x.Type != ReconnectionType.Initial)
                .Where(x => x.Type != ReconnectionType.NoMessageReceived)
                .Subscribe(me =>
                {

                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[{date}][{time}][INFO]  [{client.Name}] reconnected to gateway...");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"[{date}][{time}][INFO]  [{client.Name}] listening for events...");
                });

            client.MessageCreated
                .Subscribe(async msg =>
                {
                   
                    using var db = _dbFactory!.CreateDbContext();
                    if (msg.Message.CreatedBy == client.Id) return;
                    await msgHandler.HandleMessageAsync(msg.Message);
                    var index = msg!.Content!.IndexOf(" ");
                    
                    if (index == -1) return;
                    var message = new LocalChannelMessage()
                    {
                        Id = Guid.NewGuid(),
                        ChannelId = msg.ChannelId,
                        ServerId = msg.ServerId.ToString(),
                        AuthorId = msg.CreatedBy.ToString(),
                        MessageContent = msg.Content.Substring(index).Trim(),
                        CreatedAt = msg.CreatedAt,
                    };

                    db.Add(message);
                    db.SaveChanges();
                });

            client.MessageDeleted
                .Subscribe(async msg =>
                {
                    if (msg.CreatedBy.Equals(client.Id)) return;
                    var authorId = msg.CreatedBy;
                    var serverID = msg.ServerId;
                    var author = await msg.ParentClient.GetMemberAsync((HashId)serverID!, authorId);
                    var server = await msg.ParentClient.GetServerAsync((HashId)serverID!);
                    //TODO: handle deleted messages.
                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[{date}][{time}][INFO]  [{author.Name}] deleted msg [{msg.Content}] from [{server.Name}]");

                });

            client.MemberRemoved
                .Subscribe(async memRemoved =>
                {
                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    var serverId = memRemoved.ServerId;
                    var memId = memRemoved.Id;
                    var member = await memRemoved.ParentClient.GetMemberAsync((HashId)serverId!, memId);
                    var server = await memRemoved.ParentClient.GetServerAsync(serverId);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[{date}][{time}][INFO]  [{member.Name}] has left [{server.Name}]");
                });

           
                 

            await client.ConnectAsync();
            await client.SetStatusAsync("Watching Everything", 90002579);
            BotTimerService bts = new();
            var time = DateTime.Now.ToString(timePattern);
            var date = DateTime.Now.ToShortDateString();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"[{date}][{time}][INFO]  [{client.Name}] connected...");
            Console.WriteLine($"[{date}][{time}][INFO]  [{client.Name}] registering command modules...");
            await Task.Delay(200);
            Console.WriteLine($"[{date}][{time}][INFO]  [{client.Name}] listening for events...");
            await Task.Delay(-1);
        }

    }
}
