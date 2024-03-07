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
        private static string? timePattern = "hh:mm:ss tt";

        #region WARN
        [Command(Aliases = new string[] { "warn", "w" })]
        [Description("warns another server member with a reason for the warning.")]
        public async Task Warn(CommandEvent invokator, string user = "", string[] reason = null)
        {
            try
            {
                var embed = new Embed();
                var authorId = invokator.Message.CreatedBy;
                var serverID = invokator.Message.ServerId;
                var author = await invokator.ParentClient.GetMemberAsync((HashId)serverID!, authorId);
                if (user == "" || reason is null)
                {
                    embed.SetDescription($"<@{author.Id}> no command args found, please mention an member with a reason for the warning.");
                    await invokator.ReplyAsync(embed);
                    return;
                }
               
                var permissions = await author.GetPermissionsAsync();
                if (permissions.Contains(Permission.ManageChannels)) // if the message invokor doesn't have the correct permissions we ignore the command
                {
                    var args = string.Join(" ", reason);

                    

                    var _userId = invokator!.Mentions!.Users!.First().Id;
                    var _user = await invokator.ParentClient.GetMemberAsync((HashId)serverID!, _userId);
                    var userXp = await invokator.ParentClient.AddXpAsync((HashId)serverID!, _userId, 0);
                    var warnedUser = dbContext!.ServerMembers!.Where(x => x.Nickname == _user.Name)
                        .Select(x => x).FirstOrDefault();

                    if (warnedUser!.Warnings >= 4)
                    {
                        await invokator.ParentClient.RemoveMemberAsync((HashId)serverID!, _user.Id);
                        await invokator.ParentClient.AddMemberBanAsync((HashId)serverID!, _user.Id);
                    }
                    else
                    {
                        if (warnedUser is not null)
                        {
                            warnedUser!.Warnings += 1;
                            embed.SetDescription($"<@{_user.Id}> this is a warning!");
                            embed.AddField(new EmbedField("Issued By:", $"{author.Name}", true));
                            embed.AddField(new EmbedField("Issued To:", $"{_user.Name}", true));
                            embed.AddField(new EmbedField("Reason:", $"{args}", false));
                            embed.AddField(new EmbedField("Warnings:", $"{warnedUser!.Warnings}", false));
                            dbContext.Update(warnedUser);
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
                            newUser.RoleIds = new List<uint>();

                            await dbContext.AddAsync(newUser);
                            await dbContext.SaveChangesAsync();

                            embed.SetDescription($"<@{_user.Id}> this is a warning!");
                            embed.AddField(new EmbedField("Issued By:", $"{author.Name}", true));
                            embed.AddField(new EmbedField("Issued To:", $"{_user.Name}", false));
                            embed.AddField(new EmbedField("Reason:", $"{args}", false));
                            embed.AddField(new EmbedField("Warnings:", $"{newUser.Warnings}", false));

                        }



                        await invokator.DeleteAsync();
                        await invokator.CreateMessageAsync(embed);
                    }

                    
                }
                else
                {
                    embed = new Embed();
                    embed.SetDescription(
                        $"<@{author.Id}> you do not have the permissions to execute this command, command ignored!");

                    await invokator.Message.DeleteAsync();
                    await invokator.ReplyAsync(embed);
                }
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid();
                await invokator.ReplyAsync($"something went horrible wrong, please refer {errorId} to a mod");
            }
            
            
        }
        #endregion

        #region MUTE
        [Command(Aliases = new string[] { "mute", "m" })]
        [Description("mutes a server member with a reason why they were muted.")]
        public async Task Mute(CommandEvent invokator, string user, string[] reason)
        {
            var authorId = invokator.Message.CreatedBy;
            var serverId = invokator.ServerId;
            var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
            var authPermissions = await invokator.ParentClient.GetMemberPermissionsAsync((HashId)serverId!, authorId);

            if (authPermissions.Contains(Permission.ManageChannels))
            {
                var mutedUser = invokator!.Mentions!.Users!.First();
                var muted = await invokator.ParentClient.GetMemberAsync((HashId)serverId, mutedUser.Id);
                if (mutedUser.Id.Equals(""))
                {
                    await invokator.ReplyAsync("no mentioned user to mute, command ignored!");
                }
                else
                {
                    var roles = await invokator.ParentClient.GetMemberRolesAsync((HashId)serverId, mutedUser.Id);
                    var mutedReason = string.Join(" ", reason);
                    foreach (var r in roles)
                    {
                        try
                        { 
                            await invokator.ParentClient.RemoveMemberRoleAsync((HashId)serverId, mutedUser.Id, r);
                           
                        }
                        catch (Exception e)
                        {
                            await invokator.CreateMessageAsync($"something went horrible wrong when removing {muted.Name}'s roles\r\nerror : {e.Message}");
                            break;
                        }
                        await invokator.ParentClient.AddMemberRoleAsync((HashId)serverId!, mutedUser.Id, 36722104);
                        await invokator.CreateMessageAsync($"{muted.Name} you have been muted for : {mutedReason}");

                    }
                   
                }
            }
        }
        #endregion

        #region KICK
        [Command(Aliases = new string[] { "kick", "k" })]
        [Description("kicks another server member from the server with a reason for the kick.")]
        public async Task Kick(CommandEvent invokator, string? memToKick = "", string[]? reason = null)
        {
            var memId = invokator.Message.CreatedBy;
            var serverId = invokator.ServerId;
            var server = await invokator.ParentClient.GetServerAsync((HashId)serverId!);
            var member = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, memId);
            var permissions = await invokator.ParentClient.GetMemberPermissionsAsync((HashId)serverId!, memId);
            
            if (!permissions.Contains(Permission.RemoveMembers))
            {
                await invokator.ReplyAsync($"{member.Name} you do not have the permissions to kick members, command ignored!");
            }
            else
            {
                if (memToKick!.Equals("") || memToKick is null)
                {
                     await invokator.ReplyAsync($"{member.Name} no member name given, command ignored!");
                }
                else
                {
                    var kicked_member = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, invokator!.Mentions!.Users!.FirstOrDefault()!.Id!);
                    //await kicked_member.RemoveAsync(); 
                    //TODO: uncomment the RemoveAsync method above when done testing command.
                    var time = DateTime.Now.ToString(timePattern);
                    var date = DateTime.Now.ToShortDateString();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"[{date}][{time}][INFO]  [{invokator.ParentClient.Name}] {kicked_member.User.Name} has been removed from {server.Name}.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    var _reason = string.Join(" ", reason!);
                    var embed = new Embed()
                    {
                        Description = $"{kicked_member.Name} has been removed from {server.Name}\r\n reason : **{_reason}** ", 
                        Footer = new EmbedFooter($"{invokator.ParentClient.Name} watching everything "),
                        Timestamp = DateTime.Now
                    };
                    await invokator.ReplyAsync(embed);
                }
                
            }
        }
        #endregion

        #region BAN
        [Command(Aliases = new string[] { "ban", "b" })]
        [Description("ban another server member from the server with a reason for the ban.")]
        public async Task Ban(CommandEvent invokator, string reason)
        {

        }
        #endregion

        #region PROMOTE
        [Command(Aliases = new string[] { "promote", "pm" })]
        [Description("promotes another server member")]
        public async Task Promote(CommandEvent invokator, uint roleId, string mentionedUser = "")
        {
            var mentionedId = invokator!.Mentions!.Users!.First().Id;
            var authorId = invokator.Message.CreatedBy;
            var serverId = invokator.Message.ServerId;
            var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
            var mentioned = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, mentionedId);
            var permissions = await invokator.ParentClient.GetMemberPermissionsAsync((HashId)serverId!, authorId);

            if (!permissions.Contains(Permission.ManageRoles))
            {
                await invokator.ReplyAsync($"{author.Name} you do not have the Permissions to Promote a user, command ignored!");
            }
            else
            {
                if (mentionedUser is null || mentionedUser == "")
                {
                    await invokator.ReplyAsync($"{author.Name} please mention a user to Promote.");

                }
                else
                {
                    if (roleId == 0)
                    {
                        await invokator.ReplyAsync($"{author.Name} please mention a user to Promote.");
                    }
                    else
                    {
                        var role = await invokator.ParentClient.GetRoleAsync((HashId)serverId!, roleId);
                        await invokator.ParentClient.AddMemberRoleAsync((HashId)serverId!, mentionedId, roleId);
                        await invokator.ReplyAsync($"{role.Name} granted to {mentioned.Name}");
                    }
                    
                }
            }
           
        }
        #endregion

        #region DEMOTE
        [Command(Aliases = new string[] { "demote", "dm" })]
        [Description("demotes another server member")]
        public async Task Demote(CommandEvent invokator, string roleId)
        {

        }
        #endregion

        #region PURGE
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
               
                if (amount <= 100)
                {
                    var messages = await invokator.ParentClient.GetMessagesAsync(channelId, false, amount); // here we can specify a before date. Im thinking about it
                    foreach (var m in messages)
                    {
                        await m.DeleteAsync();
                        await Task.Delay(100);
                    }

                    var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                    var date = DateTime.Now.ToShortDateString();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[{date}][{time}][INFO]  [MODiX] {author.Name} deleted {amount} messages from [{channel.Name}]");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
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
                    var embed = new Embed()
                    {
                        Description = $"<@{author.Id}> to many messages, the limit is 100. to set limit run command **purge setlimit amount**",
                        Color = EmbedColorService.GetColor("orange", Color.Orange),
                        //Thumbnail = new EmbedMedia("https://i.imgur.com/BlC9X8b.png"),
                        Footer = new EmbedFooter($"{invokator.ParentClient.Name} watching everything."),
                        Timestamp = DateTime.Now
                    };
                    await invokator.ReplyAsync(embed);
                }
                   
                
            }
            else
            {
                await invokator.ReplyAsync($"<@{author.Id}> you do not have the permission to manage messages, command ignored!");
            }
        }
        #endregion

        #region SET
        [Command(Aliases = new string[] { "set" })]
        [Description("set [limit, cooldown, timeout] ")]
        public async Task Set(CommandEvent invokator, string command = "", int amount = 0)
        {
            var authorId = invokator.Message.CreatedBy;
            var serverID = invokator.Message.ServerId;
            var member = await invokator.ParentClient.GetMemberAsync((HashId)serverID!, authorId);
            var permissions = await invokator.ParentClient.GetMemberPermissionsAsync((HashId)serverID!, authorId);
            var embed = new Embed();

            if (permissions.Contains(Permission.ManageServer))
            {
                if (command == null || command == "")
                {
                    await invokator.ReplyAsync($"{member.Name} I did not receive the command, I was expecting something like [limit] [cooldown] or [timeout]");
                }
                else
                {
                    switch (command)
                    {
                        case "limit":
                             await invokator.ReplyAsync($"{member.Name} limit set [{amount}]");
                            break;
                        case "cooldown":
                             await invokator.ReplyAsync($"{member.Name} cooldown set [{amount}]!");
                            break;
                        case "timeout":
                             await invokator.ReplyAsync($"{member.Name} timeout set [{amount}]!");
                            break;
                        default:
                            await invokator.ReplyAsync($"{member.Name} no matching command found!");
                            break;
                    }
                }
            }
            else
            {
                await invokator.ReplyAsync($"{member.Name} you don't have the permission's to run this command, command ignored!");
            }
        }
        #endregion
    }
}
