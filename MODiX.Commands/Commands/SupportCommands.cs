using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using Guilded.Permissions;
using Guilded.Servers;

namespace MODiX.Commands.Commands
{
    public class SupportCommands : CommandModule
    {
        private static readonly string? timePattern = "hh:mm:ss tt";

        [Command(Aliases = [ "support", "ticket" ])]
        [Description("creates a support ticket for mods to discuss")]
        public async Task Support(CommandEvent invokator, string? type = "", string[]? content = null)
        {

            var authorId = invokator.Message.CreatedBy;
            var serverId = invokator.Message.ServerId;
            var member = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
            if (type.Equals("") || content is null)
            {
                await invokator.ReplyAsync($"{member.Name} command arguments not found, " +
                    $"I was expecting a ticket type and the ticket content but found nothing. " +
                    $"run command m?help <ticket> to see the ticket commands");
            }
            else
            {
                try
                {
                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    await invokator.ReplyAsync($"<@{member.Name}> a support channel hase been created, the mod's will discuss what will be done!");
                    var channel = await invokator.ParentClient.CreateChannelAsync((HashId)serverId, "support", ChannelType.Chat, "support");
                    var channelId = channel.Id;
                    var ticketContent = string.Join(" ", content);
                    var embed = new Embed();
                    embed.SetTitle($"{type.ToUpper()} ticket created by {member.Name}");
                    embed.SetDescription($"[{date}][{time}]: Ticket Type: {type.ToUpper()}\r\n{ticketContent}\r\n\r\n__this will ping all members with the mod role or support role__\r\n<@36676536>");
                    embed.SetFooter("MODiX watching everything ");
                    embed.SetTimestamp(DateTime.Now);
                    await invokator.ParentClient.CreateMessageAsync(channelId, embed);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }

        [Command(Aliases = new string[] { "set" })]
        [Description("set is designed to handle different commands. like [set] [prefix], [set] [supportchannel] ect.")]
        public async Task Set(CommandEvent invokator, string command, string context)
        {

        }
    }
}
