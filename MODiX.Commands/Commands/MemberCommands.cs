using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using Guilded.Commands.Items;
using Guilded.Servers;
using Guilded.Users;
using MODiX.Services.Features.Music;
using MODiX.Services.Services;

namespace MODiX.Commands.Commands
{
    public class MemberCommands : CommandModule
    {
        //member commands
        // uptime, profile (mentioned member profile), help, serverInfo, wikipedia search, meme
        private MusicPlayerProvider player = new();
        private static string? timePattern = "hh:mm:ss tt";
        [Command(Aliases = new string[] { "alive", "uptime", "online" })]
        [Description("returns how long the bot has been online since the last restart")]
        public async Task Uptime(CommandEvent ctx)
        {
            var uptime = BotTimerService.GetBotUptime();
            var embed = new Embed()
            {
                Title = $"{ctx.ParentClient.Name} has been online for {uptime}",
                Color = EmbedColorService.GetColor("teal", Color.Teal),
                Footer = new EmbedFooter($"{ctx.ParentClient.Name} watching everything."),
                Timestamp = DateTime.Now
            };

            await ctx.CreateMessageAsync(embed);
        }

        [Command(Aliases = new string[] { "profile" })]
        [Description("gets the triggering users member info")]
        public async Task Profile(CommandEvent invokator, string mentions)
        {
            try
            {
                var authorId = invokator.Message.CreatedBy;
                var serverId = invokator.ServerId;
                var user = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, invokator.Mentions!.Users!.First().Id);
                var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
                var server = await invokator.ParentClient.GetServerAsync((HashId)serverId);
                var xp = await invokator.ParentClient.AddXpAsync((HashId)serverId, user.Id, 0);
                // need to write an api service to retrieve the members servers. wip
                var embed = new Embed();
                embed.SetDescription($"Profile for <@{user.Id}> requested by <@{authorId}>");
                embed.SetThumbnail(user.Avatar!.AbsoluteUri);
                embed.AddField("Name", $"<@{user.Id}>", false);
                embed.AddField("Joined", user.JoinedAt, true);
                embed.AddField("Created", user.CreatedAt, true);
                embed.AddField("XP", xp, true);
                embed.AddField("Server", server.Name, true);

                await invokator.CreateMessageAsync(embed);

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
                var embed = new Embed();

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

                var embed = new Embed();
                embed.SetTitle($"{botName} Info");
                embed.AddField("Server Id", $"`{serverId}`", true);
                embed.AddField("Bot Id", $"`{botId}`", true);
                embed.AddField("Bot Name", $"`{botName}`", true);
                embed.AddField("Uptime", $"`{botUptime}`", true);
                embed.AddField("Creator", $"`{creatorName}`", true);
                embed.SetThumbnail(botAvatar);
                embed.SetColor(EmbedColorService.GetColor("blurple", Color.MediumPurple));
                embed.SetFooter($"{botName} watching everything ");
                embed.SetTimestamp(DateTime.Now);

                await invokator.ReplyAsync(embed);

            }
            catch (Exception e)
            {
                var time = DateTime.Now.ToString(timePattern);
                var date = DateTime.Now.ToShortDateString();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[{date}][{time}][ERROR]  [MODiX] [{e.Message}]");
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
}
}
