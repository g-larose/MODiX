using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using Guilded.Commands.Items;
using Microsoft.EntityFrameworkCore;
using MODiX.Data.Factories;
using MODiX.Services.Features._8Ball;
using MODiX.Services.Features.Music;
using MODiX.Services.Features.Welcomer;
using MODiX.Services.Services;
using Humanizer;

namespace MODiX.Commands.Commands
{
    public class MemberCommands : CommandModule
    {
        //member commands
        // uptime, profile (mentioned member profile), help, serverInfo
        private readonly ModixDbContextFactory dbFactory = new();
        private readonly ServerMemberService memService = new();
        private MusicPlayerProvider player = new();
        private static string? timePattern = "hh:mm:ss tt";

        [Command(Aliases = new string[] { "alive", "uptime", "online" })]
        [Description("returns how long the bot has been online since the last restart")]
        public async Task Uptime(CommandEvent ctx)
        {
            var uptime = BotTimerService.GetBotUptime();
            var sw = Stopwatch.StartNew();
            using var db = dbFactory.CreateDbContext();
            var user = db!.ServerMembers!.First();
            sw.Stop();
            var dbLatency = sw.ElapsedMilliseconds;
            
            sw.Start();
            var ping = new Ping();
            await ping.SendPingAsync("google.com");
            sw.Stop();
            var pingTime = sw.ElapsedMilliseconds;

            var embed = new Embed()
            {
                Title = $"{ctx.ParentClient.Name} has been online for {uptime}",
                Color = EmbedColorService.GetColor("teal", Color.Teal),
                Footer = new EmbedFooter($"{ctx.ParentClient.Name} watching everything"),
                Timestamp = DateTime.Now
            };
            embed.AddField("Db Latency", $"{dbLatency}ms", true);
            embed.AddField("Ping Reply", $"{pingTime}ms", true);

            await ctx.CreateMessageAsync(embed);
        }

        [Command(Aliases = new string[] { "profile" })]
        [Description("gets the triggering users member info")]
        public async Task Profile(CommandEvent invokator, string mentions = "")
        {
            try
            {
                if (mentions is null || mentions.Equals(""))
                {
                    var authorId = invokator.Message.CreatedBy;
                    var serverId = invokator.ServerId;
                    var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
                    var server = await invokator.ParentClient.GetServerAsync((HashId)serverId);
                    var xp = await invokator.ParentClient.AddXpAsync((HashId)serverId, author.Id, 0);
                   // var servers = await memService.GetMemberServersAsync(serverId.ToString()!);
                    using var db = dbFactory.CreateDbContext();
                    var localUser = db!.ServerMembers!.Where(x => x.UserId!.Trim().Equals(author.Id.ToString())).Include(x => x.Wallet).ToList();

                    if (localUser is null || localUser.Count < 1)
                    {
                        await invokator.ReplyAsync($"{author.Name} is not in the database, please add {author.Name} before running profile command");
                        return;
                    }
                    else
                    {
                        var warnings = localUser.Select(x => x.Warnings).First();
                        var points = localUser?.First()!.Wallet!.Points.ToWords();
                        var embed = new Embed();
                        embed.SetDescription($"Profile for <@{author.Id}> requested by <@{authorId}>");
                        embed.SetThumbnail(author.Avatar!.AbsoluteUri);
                        embed.AddField("Name", $"<@{author.Id}>", false);
                        embed.AddField("Joined", author.JoinedAt.Humanize(), true);
                        embed.AddField("Created", author.CreatedAt.Humanize(), true);
                        embed.AddField("XP", xp, true);
                        embed.AddField("Wallet", points, true);
                        embed.AddField("Warnings", warnings.ToString(), true);
                        embed.AddField("Server", server.Name, true);

                        await invokator.ReplyAsync(embed);
                    }
                    
                }
                else
                {
                    var authorId = invokator.Message.CreatedBy;
                    var serverId = invokator.ServerId;
                    var user = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, invokator.Mentions!.Users!.First().Id);
                    var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
                    var server = await invokator.ParentClient.GetServerAsync((HashId)serverId);
                    var xp = await invokator.ParentClient.AddXpAsync((HashId)serverId, user.Id, 0);

                    using var db = dbFactory.CreateDbContext();
                    var localUser = db!.ServerMembers!.Where(x => x.UserId!.Trim().Equals(author.Id.ToString())).Include(x => x.Wallet).ToList();

                    if (localUser is null || localUser.Count < 1)
                    {
                        await invokator.ReplyAsync($"{author.Name} is not in the database, please add {author.Name} before running profile command");
                        return;
                    }
                    else
                    {
                        var warnings = localUser.Select(x => x.Warnings).FirstOrDefault();
                        var points = localUser?.First()!.Wallet!.Points.ToWords();
                        var embed = new Embed();
                        embed.SetDescription($"Profile for <@{user.Id}> requested by <@{authorId}>");
                        embed.SetThumbnail(user.Avatar!.AbsoluteUri);
                        embed.AddField("Name", $"<@{user.Id}>", false);
                        embed.AddField("Joined", user.JoinedAt.Humanize(), true);
                        embed.AddField("Created", user.CreatedAt.Humanize(), true);
                        embed.AddField("XP", xp, true);
                        embed.AddField("Wallet", points, true);
                        embed.AddField("Warnings", warnings.ToString(), true);
                        embed.AddField("Server", server.Name, true);

                        await invokator.ReplyAsync(embed);

                    }
                    
                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        [Command(Aliases = new string[] { "help" })]
        [Description("list of bot commands")]
        public async Task Help(CommandEvent invokator)
        {
            try
            {
                var authorId = invokator.Message.CreatedBy;
                var serverId = invokator.ServerId;
                var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
                var bot = invokator.ParentClient.Name;
                var embed = new Embed();
                embed.SetDescription($"**Member Commands**\r\n1. Uptime: get the online time for {invokator.ParentClient.Name}\r\n" +
                    $"2. Profile: get the profile for a server member\r\n" +
                    $"3. Welcome: welcome a new member to the server\r\n" +
                    $"4. BotInfo: get {bot}'s information\r\n" +
                    $"5. ServerInfo: get the server's information\r\n" +
                    $"6. 8Ball: ask 8ball a question, get a response.\r\n\r\n" +
                    $"**Mod Commands**\r\n1. Purge: remove a set amount of messages from a channel\r\n" +
                    $"2. Mute: mute a member for a set amount of time\r\n" +
                    $"3. Warn: warn a member, this adds a warning to the member in the database\r\n" +
                    $"4. Kick: remove a member from the server\r\n" +
                    $"5. Ban: ban a member from the server");
                embed.SetFooter(new EmbedFooter($"{bot} watching everything\r\n"));
                embed.SetTimestamp(DateTime.Now);

                await invokator.ReplyAsync(embed);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            
        }


        [Command(Aliases = new string[] { "botinfo", "binfo" })]
        [Description("list the bot info")]
        public async Task BotInfo(CommandEvent invokator)
        {
            try
            {
                var botId = invokator.ParentClient.Id;
                var serverId = invokator.Message.ServerId;
                var bot = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, (HashId)botId!);
                var botAvatar = bot.Avatar!;
                var botName = bot.Name;
                var botUptime = BotTimerService.GetBotUptime();
                var creatorName = "Async<RogueLabs>";

                var uptime = BotTimerService.GetBotUptime();
                var sw = Stopwatch.StartNew();
                using var db = dbFactory.CreateDbContext();
                var user = db!.ServerMembers!.First();
                sw.Stop();
                var dbLatency = sw.ElapsedMilliseconds;

                sw.Start();
                var ping = new Ping();
                await ping.SendPingAsync("google.com");
                sw.Stop();
                var pingTime = sw.ElapsedMilliseconds;

                var embed = new Embed();
                embed.SetTitle($"{botName} Info");
                embed.AddField("Creator", $"<@mq1ezklm>", false);
                embed.AddField("Server Id", $"`{serverId}`", true);
                embed.AddField("Bot Id", $"`{botId}`", true);
                embed.AddField("Bot Name", $"`{botName}`", true);
                embed.AddField("Uptime", $"`{botUptime}`", true);
                embed.AddField("Db Ping", $"`{dbLatency}ms`", true);
                embed.AddField("Api Ping", $"`{pingTime}ms`", true);
                embed.SetThumbnail(botAvatar);
                embed.SetColor(EmbedColorService.GetColor("grey", Color.DarkGray));
                embed.SetFooter($"{botName} watching everything ");
                embed.SetTimestamp(DateTime.Now);

                await invokator.ReplyAsync(embed);

            }
            catch (Exception e)
            {
                var time = DateTime.Now.ToString(timePattern);
                var date = DateTime.Now.ToShortDateString();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[{date}][{time}][ERROR]  [{invokator.ParentClient.Name}] [{e.Message}]");
                await invokator.ReplyAsync(
                    $"`[ERROR]` something went wrong while fetching bot info. please try again later.");
                return;
            }
            
        }

        //test command for playing youtube music...doesn't work yet
        [Command("play")]
        [Description("play a youtube song")]
        public async Task PlayAudio(CommandEvent invokator)
        {
           // await player.PlayVideoAsync(); this is the play command base method. not working yet well
           //it works kind of...no errors but it doesnt play the audio yet.
           var authorId = invokator.Message.CreatedBy;
           var serverId = invokator.ServerId;
           var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);

           var embed = new Embed();
           var embedColor = EmbedColorService.GenerateRandomEmbedColor();
           embed.SetDescription(
               $"I'm sorry <@{authorId}> this feature is still being worked on and isn't available yet!");
           embed.SetThumbnail(author.Avatar!);
           embed.SetColor(Color.FromArgb(embedColor));
           embed.SetFooter($"{invokator.ParentClient.Name} ");
           embed.SetTimestamp(DateTime.Now);
           await invokator.ReplyAsync(embed);
        }

        [Command(Aliases = new string[] { "welcome" })]
        [Description("says a random welcome message")]
        public async Task Welcome(CommandEvent invokator, string userToWelcome = "")
        {
            if (userToWelcome is null || userToWelcome == "")
            {

            }
            else
            {
                using var welcomerService = new WelcomerProviderService();
                var welcomeMsg = await welcomerService.GetRandomWelcomeMessageAsync();
                var serverId = invokator.ServerId;
                var server = await invokator.ParentClient.GetServerAsync((HashId)serverId!);
                var userId = invokator!.Mentions!.Users!.FirstOrDefault()!.Id!;
                var user = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, userId);
                var replyTo = new List<Guid>();
                var replyToGuid = Guid.TryParse(user.Id.ToString(), out Guid g);
                replyTo.Add(g);
                if (welcomeMsg is not null || welcomeMsg!.Message != "")
                {
                    var newMsg = welcomeMsg!.Message!.Replace("[member]", userToWelcome).Replace("[server]", server.Name);
                    await invokator.CreateMessageAsync($"{newMsg}");
                }
            }
            
        }

        [Command(Aliases = new[] { "serverinfo" })]
        [Description("get the server info")]
        public async Task ServerInfo(CommandEvent invokator)
        {
            using var serverService = new ServerMemberService();
            var serverId = invokator.ServerId;
            var client = invokator.ParentClient;
            var server = await invokator.ParentClient.GetServerAsync((HashId)serverId!);
            var ownerId = server.OwnerId;
            var owner = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, ownerId);
            var members = await serverService.GetServerMembersAsync(client, (HashId)serverId!);

            var embed = new Embed();
            embed.SetTitle($"{server.Name} Info");
            embed.AddField("Server Name", server.Name, true);
            embed.AddField("Created", server.CreatedAt.Humanize(), true);
            embed.AddField("Owner", owner.Name, true);
            embed.AddField("Member Count", members.Count, true);
            embed.SetFooter($"{invokator.ParentClient.Name} watching everything ");
            embed.SetTimestamp(DateTime.Now);

            await invokator.ReplyAsync(embed);
            ServerMemberService.Dispose();
        }

        [Command(Aliases = new[] { "8ball" })]
        [Description("ask 8ball a question, get a response message")]
        public async Task EightBall(CommandEvent invokator, string[] query)
        {
            if (query is not null || query!.Length > 0)
            {
                using var eightBall = new _8BallProviderService();
                var question = string.Join(" ", query);
                var authorId = invokator.Message.CreatedBy;
                var serverId = invokator.Message.ServerId;
                var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
                var response = eightBall.GetEightBallResponse();

                var embed = new Embed();
                embed.SetDescription($"{response}");
                embed.SetColor(EmbedColorService.GetColor("gray", Color.DarkGray));
                //embed.SetFooter($"{invokator.ParentClient.Name} ");
                //embed.SetTimestamp(DateTime.Now);

                await invokator.ReplyAsync(embed);
            }
            else
            {
                await invokator.ReplyAsync($"I'm sorry but I don't understand the question, please try again later.");
            }
            
        }

        [Command(Aliases = new string[] { "ping" })]
        [Description("get a ping , pong response with how long it took in ms")]
        public async Task Ping(CommandEvent invokator)
        {
            var timer = new Stopwatch();
            timer.Start();
            var reply = await invokator.ReplyAsync("Pong...");
            timer.Stop();
            await reply.UpdateAsync($"Pong....took {timer.ElapsedMilliseconds}ms");
        }

        [Command(Aliases = new string[] { "addmember", "addmem" })]
        [Description("add a member to the Database")]
        public async Task AddMember(CommandEvent invokator, string mentionedMember = "")
        {
            var memberId = invokator!.Message.CreatedBy;
            var mentionedUserId = invokator!.Mentions!.Users!.First().Id;
            var serverId = invokator!.ServerId!;
            var user = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, mentionedUserId);
            var permissions = await invokator.ParentClient.GetMemberPermissionsAsync((HashId)serverId!, memberId);
            var timer = new Stopwatch();
            timer.Start();
            if (!permissions.Contains(Guilded.Permissions.Permission.ManageServer))
            {
                timer.Stop();

                await invokator.ReplyAsync($"{user.Name} you do not have the appropiate permission to manage members, command ignored! took... **{timer.ElapsedMilliseconds}**ms to execute");
                
                return;
            }
            else
            {
                if (mentionedMember == "")
                {
                    timer.Stop();
                    await invokator.ReplyAsync($"no member to add, command ignored! took... **{timer.ElapsedMilliseconds}**ms to fire");
                }
                else
                {


                    var result = await memService.AddServerMemberToDBAsync(invokator.ParentClient, user);
                    if (result)
                    {
                        var db = dbFactory.CreateDbContext();
                        var newMem = db.ServerMembers!.Where(x => x.UserId == memberId.ToString()).Include(x => x.Wallet).ToList();
                        timer.Stop();
                        await invokator.ReplyAsync($"member {user.Name} has been added to the db with [{newMem.First()!.Wallet!.Points}] points added to their wallet, command took... **{timer.ElapsedMilliseconds}**ms to execute");
                    }
                    else
                    {
                        timer.Stop();
                        await invokator.ReplyAsync($"member already exists in the db, command ignored! command took... **{timer.ElapsedMilliseconds}**ms to execute");
                    }
                        

                }
            }
            
        }
}
}
