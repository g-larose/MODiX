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
using MODiX.Services.Services;

namespace MODiX.Commands.Commands
{
    public class MemberCommands : CommandModule
    {
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
}
}
