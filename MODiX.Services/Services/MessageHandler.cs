using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Client;
using Guilded.Content;
using Guilded.Permissions;
using Microsoft.VisualBasic;
using MODiX.Data.Models;
using MODiX.Services.BaseModules;
using MODiX.Services.Interfaces;

namespace MODiX.Services.Services
{
    public class MessageHandler : IMessageHandler, IDisposable
    {
        public Message? Message { get; set; }
        public AbstractGuildedClient? Client { get; set; }
        private string? MessageAuthor { get; set; }
        private uint MessageCount { get; set; }

        private string? timePattern = "hh:mm:ss tt";

        public MessageHandler(AbstractGuildedClient? client, Message? message = null)
        {
            Message = message;
            Client = client;
        }

        public async Task HandleMessageAsync(Message message)
        {
            this.Message = message;
            var authorId = message.CreatedBy;
            var serverId = message.ServerId;
            var channelId = message.ChannelId;
            var embed = new Embed();
            try
            {
                var author = await message.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
                if (author.IsBot) return;

                if (message!.Content!.Equals(Message!.Content))
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
                    Message = message;
                }

                //filter the links from the message
                var filtered = await FilterMessageAsync(message);
                Dispose();
            }
            catch (Exception e)
            {
                var time = DateTime.Now.ToString(timePattern);
                var date = DateTime.Now.ToShortDateString();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[{date}][{time}][ERROR]  [{message.ParentClient.Name}] {e.Message} [MessageHandler event]");
            }
            
        }

        #region FILTER MESSAGE
        private async Task<Result<bool, string>> FilterMessageAsync(Message msg)
        {
            var pattern = @"(?:https?|ftp):\/\/(?:[\w-]+\.)+[\w-]+(?:\/[\w@?^=%&/~+#-]*)?|guilded\.gg|discord\.gg";
            var regex = new Regex(pattern);

            if (regex.IsMatch(msg.Content!))//convert this into a switch expression to handle different senerios.
            {
                if (msg.ChannelId.Equals(""))
                    return Result<bool, string>.Err("could not find channel")!;
                var author = await msg.ParentClient.GetMemberAsync((HashId)msg.ServerId!, msg.CreatedBy);
                var channelId = msg.ChannelId;
                var permissions = await author.GetPermissionsAsync();
                if (!permissions.Contains(Permission.ManageChannels))
                {
                    var embed = new Embed();
                    await msg.ParentClient.DeleteMessageAsync(channelId, msg.Id);
                    embed.SetDescription($"<@{author.Id}> your message contained block content and was removed");
                    embed.SetColor(EmbedColorService.GetColor("gray", Color.Gray));
                    await msg.ReplyAsync(embed);
                    return Result<bool, string>.Ok(true)!;
                }
            }
            return false;
        }
        #endregion

        #region DISPOSE
        public void Dispose()
        {
            DisposableBase disposableBase = new();
            disposableBase.Dispose();
        }
        #endregion
    }
}
