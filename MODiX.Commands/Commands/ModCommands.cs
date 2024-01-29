using System.Drawing;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using Guilded.Permissions;
using MODiX.Services.Services;

namespace MODiX.Commands.Commands
{
    public class ModCommands : CommandModule
    {
        [Command(Aliases= new string[] { "warn", "w" })]
        [Description("warns another server member with a reason for the warning.")]
        public async Task Warn(CommandEvent invokator, string user, string[] reason)
        {
            var authorId = invokator.Message.CreatedBy;
            var serverID = invokator.Message.ServerId;
            var author = await invokator.ParentClient.GetMemberAsync((HashId)serverID!, authorId);
            var permissions = await author.GetPermissionsAsync();
            if (permissions.Contains(Permission.ManageChannels)) // if the message invokor doesn't have the correct permissions we ignore the command
            {
                var args = string.Join(" ", reason);

                var embed = new Embed();
                embed.AddField(new EmbedField("Issued By:", $"<@{author.Id}>", true));
                embed.AddField(new EmbedField("Issued To:", $"<@{user}>", true));
                embed.AddField(new EmbedField("Reason:", $"{args}", false));

                await invokator.DeleteAsync();
                await invokator.CreateMessageAsync(embed);
            }
            else
            {
                var embed = new Embed();
                embed.SetDescription(
                    $"<@{author.Id}> you do not have the permissions to execute this command, command ignored!");

                await invokator.Message.DeleteAsync();
                await invokator.ReplyAsync(embed);
            }
            
        }

        [Command(Aliases = new string[] { "mute", "m" })]
        [Description("mutes a server member with a reason why they were muted.")]
        public async Task Mute(CommandEvent invokator, string reason)
        {

        }

        [Command(Aliases = new string[] { "kick", "k" })]
        [Description("kicks another server member from the server with a reason for the kick.")]
        public async Task Kick(CommandEvent invokator, string reason)
        {

        }

        [Command(Aliases = new string[] { "ban", "b" })]
        [Description("ban another server member from the server with a reason for the ban.")]
        public async Task Ban(CommandEvent invokator, string reason)
        {

        }

        [Command(Aliases = new string[] { "promote", "pm" })]
        [Description("promotes another server member")]
        public async Task Promote(CommandEvent invokator, string roleId)
        {

        }

        [Command(Aliases = new string[] { "demote", "dm" })]
        [Description("demotes another server member")]
        public async Task Demote(CommandEvent invokator, string roleId)
        {

        }

        [Command(Aliases = new string[] { "updateDC", "udc" })]
        [Description("update the default channel")]
        public async Task UpdateDefaultChannel(CommandEvent invokator, string serverId, string channelId)
        {

        }

        [Command(Aliases = new string[] { "purge", "p" })]
        [Description("removes a set amount of channel messages")]
        public async Task Purge(CommandEvent invokator, uint amount)
        {
            var authorId = invokator.Message.CreatedBy;
            var serverId = invokator.Message.ServerId;
            var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
            var permissions = await invokator.ParentClient.GetMemberPermissionsAsync((HashId)serverId!, authorId);

            if (permissions.Contains(Permission.ManageMessages))
            {
                var channelId = invokator.ChannelId;
                var channel = await invokator.ParentClient.GetChannelAsync(channelId);
                var messages = await invokator.ParentClient.GetMessagesAsync(channelId, false, amount);
                for (int i = 0; i < messages.Count; i++)
                {
                    await messages[i].DeleteAsync();
                    await Task.Delay(100);
                }

                var timeStamp = string.Join(" ", DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToLongTimeString());
                Console.WriteLine($"[{timeStamp}] [INFO] [MODiX] {author.Name} deleted {amount} messages from [{channel.Name}]");
                var embed = new Embed()
                {
                    Description = $"{amount} messages deleted",
                    Color = EmbedColorService.GetColor("orange", Color.Orange),
                    Footer = new EmbedFooter($"{invokator.ParentClient.Name} watching everything."),
                    Timestamp = DateTime.Now
                };
                var deleteMessage = await invokator.CreateMessageAsync(embed);
            }
            else
            {
                await invokator.ReplyAsync($"`{author}` you do not have the permission to manage messages, command ignored!");
            }
        }
    }
}
