using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Content;
using Guilded.Permissions;
using Microsoft.VisualBasic;
using MODiX.Services.Interfaces;

namespace MODiX.Services.Services
{
    public class MessageHandler : IMessageHandler
    {
        private string? MessageAuthor { get; set; }
        private string? Message { get; set; }
        private uint MessageCount { get; set; }

        private string? timePattern = "hh:mm:ss tt";

        public async Task HandleMessageAsync(Message message)
        {
            var authorId = message.CreatedBy;
            var serverId = message.ServerId;
            var channelId = message.ChannelId;
            var embed = new Embed();
            try
            {
                var author = await message.ParentClient.GetMemberAsync((HashId)serverId!, authorId);

                if (message!.Content!.Equals(Message))
                {
                    MessageAuthor = author.Name;
                    MessageCount++;
                    if (MessageCount > 3)
                    {
                        var messages = await message.ParentClient.GetMessagesAsync(channelId, false, MessageCount - 1);
                        foreach (var mes in messages)
                        {
                            await mes.DeleteAsync();
                            await Task.Delay(100);
                        }

                        
                        embed.SetDescription($"<@{author.Id}> you are sending the same message to fast, slow down or you will be muted!");
                        embed.SetColor(EmbedColorService.GetColor("gray", Color.Gray));
                        await message.ReplyAsync(embed);
                        MessageCount = 0;
                        MessageAuthor = "";
                        Message = null;
                    }
                }
                else
                {
                    MessageCount = 1;
                    MessageAuthor = author.Name;
                    Message = message.Content;
                }


                //var pattern = @"https?://\S+|guilded.gg|www\S+|http?://\S+";
                var pattern = @"(?:https?|ftp):\/\/(?:[\w-]+\.)+[\w-]+(?:\/[\w@?^=%&/~+#-]*)?|guilded\.gg|discord\.gg";
                var regex = new Regex(pattern);

                if (regex.IsMatch(message.Content!))//convert this into a switch expression to handle different senerios.
                {
                    if (message.ChannelId.Equals(""))
                        return;
                    var permissions = await author.GetPermissionsAsync();
                    if (!permissions.Contains(Permission.ManageChannels))
                    {
                        await message.ParentClient.DeleteMessageAsync(channelId, message.Id);
                        embed.SetDescription($"<@{author.Id}> your message contained block content and was removed");
                        embed.SetColor(EmbedColorService.GetColor("gray", Color.Gray));
                        await message.ReplyAsync(embed);
                    }
                }
            }
            catch (Exception e)
            {
                var time = DateTime.Now.ToString(timePattern);
                var date = DateTime.Now.ToShortDateString();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[{date}][{time}][ERROR]  [{message.ParentClient.Name}] {e.Message} [MessageHandler event]");
            }
            
        }
    }
}
