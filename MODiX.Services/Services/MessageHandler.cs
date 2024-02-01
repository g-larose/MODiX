using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Guilded.Base;
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

        public async Task HandleMessageAsync(Message message)
        {
            var authorId = message.CreatedBy;
            var serverId = message.ServerId;
            var channelId = message.ChannelId;
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
                    
                    await message.ReplyAsync(
                        $"{MessageAuthor} you are sending the same message to fast, slow down or you will be muted! {MessageCount}");
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
            var pattern = @"https?://\S+|guilded.gg|www\S+|http?://\S+";
            var regex = new Regex(pattern);

            if (regex.IsMatch(message.Content!))//convert this into a switch expression to handle different senerios.
            {
                var permissions = await author.GetPermissionsAsync();
                if (!permissions.Contains(Permission.ManageChannels))
                {
                    await message.ParentClient.DeleteMessageAsync(channelId, message.Id);
                    await message.ReplyAsync(
                        $"`{author.Name}` your message contained block content and was removed");
                }
            }
            else
            {
                
            }
        }
    }
}
