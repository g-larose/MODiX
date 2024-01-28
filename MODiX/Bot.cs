using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Guilded;
using Guilded.Base;
using Guilded.Client;
using Guilded.Commands;
using Guilded.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MODiX.Commands.Commands;
using MODiX.Config;
using MODiX.Data.Config;
using MODiX.Data.Factories;
using Websocket.Client;

namespace MODiX
{
    public class Bot
    {
        private static string? json   = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json"));
        private static string? token  = JsonSerializer.Deserialize<ConfigJson>(json!)!.Token!;
        private static string? prefix = JsonSerializer.Deserialize<ConfigJson>(json!).Prefix!;
       
        public async Task RunAsync()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            await using var client = new GuildedBotClient(token!)
                .AddCommands(new ModCommands(), prefix!);

            client.Prepared
                .Subscribe(async me =>
                {
                    var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                    var date = DateTime.Now.ToShortDateString();
                    Console.WriteLine($"[{date}] [{time}] [{me.ParentClient.Name}] talking to gateway...");
                });

            client.MemberJoined
                .Subscribe(async memJoined =>
                {
                    var serverId = memJoined.ServerId;
                    var server = await memJoined.ParentClient.GetServerAsync((HashId)serverId);
                    var defaultChannelId = server.DefaultChannelId;
                    await memJoined.ParentClient.CreateMessageAsync((Guid)defaultChannelId!,
                        $"Welcome to the server, `{memJoined.Name}`\r\nplease visit #%rules% to read our code of conduct.", null, null, null, true, false);
                });

            client.Disconnected
                .Where(e => e.Type != DisconnectionType.NoMessageReceived)
                .Subscribe(me =>
                {
                    var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                    var date = DateTime.Now.ToShortDateString();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"[{date}] [{time}] [ERROR] [MODiX] disconnected from gateway...");
                });

            client.Reconnected
                .Where(x => x.Type != ReconnectionType.Initial)
                .Where(x => x.Type != ReconnectionType.NoMessageReceived)
                .Subscribe(me =>
                {
                    var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                    var date = DateTime.Now.ToShortDateString();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[{date}] [{time}] [INFO]   [MODiX] reconnected to gateway...");
                });


            await client.ConnectAsync();
            await client.SetStatusAsync("Watching Everything", 90002579);
            var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
            var date = DateTime.Now.ToShortDateString();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"[{date}] [{time}] [MODiX] connected...");
            Console.WriteLine($"[{date}] [{time}] [MODiX] registering command modules...");
            await Task.Delay(200);
             Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"[{date}] [{time}] [MODiX] listening for events...");
            await Task.Delay(-1);
        }

        private void ConfigureServices()
        {
            IHost _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddDbContextFactory<ModixDbContext>();
            }).Build();

            _host.Start();
        }
    }
}
