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
        public async Task HandleMessageAsync(Message message)
        {
            var pattern = @"https?://\S+|guilded.gg|www\S+|http?://\S+";
            var regex = new Regex(pattern);
            if (regex.IsMatch(message.Content!))//convert this into a switch expression to handle different senerios.
            {
                var channelId = message.ChannelId;
                var serverId = message.ServerId;
                var authorId = message.CreatedBy;
                var author = await message.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
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
