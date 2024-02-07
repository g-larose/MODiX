using System.Drawing;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using Guilded.Content;
using Guilded.Permissions;
using Guilded.Users;
using MODiX.Data;
using MODiX.Data.Models;
using MODiX.Services.Services;

namespace MODiX.Commands.Commands
{
    public class ModCommands : CommandModule
    {
        private readonly ModixDbContext dbContext = new ModixDbContext();

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
               
                var _userId = invokator!.Mentions!.Users!.First().Id;
                var _user = await invokator.ParentClient.GetMemberAsync((HashId)serverID!, _userId);
                var userXp = await invokator.ParentClient.AddXpAsync((HashId)serverID!, _userId, 0);
                var warnedUser = dbContext!.ServerMembers!.Where(x => x.Nickname == _user.Name).ToList();
                if (warnedUser.Count > 0)
                {
                    warnedUser!.FirstOrDefault()!.Warnings += 1;
                    embed.AddField(new EmbedField("Issued By:", $"<@{author.Id}>", true));
                    embed.AddField(new EmbedField("Issued To:", $"<@{warnedUser!.FirstOrDefault()!.Nickname}>", true));
                    embed.AddField(new EmbedField("Reason:", $"{args}", false));
                    embed.AddField(new EmbedField("Warnings:", $"{warnedUser!.FirstOrDefault()!.Warnings + 1}", false));
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    var newUser = new LocalServerMember();
                    newUser.Id = Guid.NewGuid();
                    newUser.UserId = _user.Id.ToString();
                    newUser.ServerId = serverID.ToString();
                    newUser.Nickname = _user.Name;
                    newUser.CreatedAt = DateTime.UtcNow;
                    newUser.JoinedAt = _user.JoinedAt;
                    newUser.Warnings += 1;
                    newUser.Messages = null;
                    newUser.Xp = int.Parse(userXp.ToString());
                    newUser.RoleIds = Array.Empty<int>();

                    await dbContext.AddAsync(newUser);
                    await dbContext.SaveChangesAsync();

                    embed.AddField(new EmbedField("Issued By:", $"<@{author.Id}>", true));
                    embed.AddField(new EmbedField("Issued To:", $"<@{newUser.Id}>", false));
                    embed.AddField(new EmbedField("Reason:", $"{args}", false));
                    embed.AddField(new EmbedField("Warnings:", $"{newUser.Warnings + 1}", false));

                }
                    


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
        public async Task Mute(CommandEvent invokator, string user, string[] reason)
        {
            var authorId = invokator.Message.CreatedBy;
            var serverId = invokator.ServerId;
            var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
            var authPermissions = await invokator.ParentClient.GetMemberPermissionsAsync((HashId)serverId!, authorId);

            if (!authPermissions.Contains(Permission.ManageChannels))
            {
                var mutedUser = invokator!.Mentions!.Users!.First();
                if (mutedUser.Id.Equals(""))
                {
                    await invokator.ReplyAsync("no mentioned user to mute, command ignored!");
                }
                else
                {
                }
            }
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
                var messages = await invokator.ParentClient.GetMessagesAsync(channelId, false, amount + 1); // here we can specify a before date. Im thinking about it
                
                foreach (var m in messages)
                {
                    await m.DeleteAsync();
                    await Task.Delay(100);
                }

                var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                var date = DateTime.Now.ToShortDateString();
                Console.WriteLine($"[{date}] [{time}] [INFO] [MODiX] {author.Name} deleted {amount} messages from [{channel.Name}]");
                var embed = new Embed()
                {
                    Description = $"{amount} messages deleted",
                    Color = EmbedColorService.GetColor("orange", Color.Orange),
                    Thumbnail = new EmbedMedia("https://i.imgur.com/BlC9X8b.png"),
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
