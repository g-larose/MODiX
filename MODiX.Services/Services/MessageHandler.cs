using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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
        public string? Message { get; set; }
        public AbstractGuildedClient? Client { get; set; }
        private string? MessageAuthor { get; set; }
        private uint MessageCount { get; set; }

        private string? timePattern = "hh:mm:ss tt";

        public MessageHandler(AbstractGuildedClient? client, Message? message = null)
        {
            Message = message?.Content;
            Client = client;
        }

        public async Task HandleMessageAsync(Message message)
        {
            this.Message = message?.Content;
            var authorId = message.CreatedBy;
            var serverId = message.ServerId;
            var channelId = message.ChannelId;
            var embed = new Embed();
            try
            {
                var author = await message.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
                if (author.IsBot) return;

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

                //filter the links from the message
                var filtered = await FilterMessageAsync(message);
                if (!filtered.IsOk)
                {
                    await message.DeleteAsync();
                    await message.ReplyAsync($"{author.Name} {filtered.Error} and has been removed.");
                }
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
            //7zZsro9PvWHQG64UX8nQGt61zZikoCAg
            var pattern = @"(?:https?|ftp):\/\/(?:[\w-]+\.)+[\w-]+(?:\/[\w@?^=%&/~+#-]*)?|guilded\.gg|discord\.gg";
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var postReqValues = new Dictionary<string, string>()
            {
                {"stricktness", "1" },
                {"fast", "true" }
            };

            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(postReqValues);

            foreach (Match match in regex.Matches(msg.Content))
            {
                string url = match.Value.Replace(":", "%3A").Replace("/", "%2F");
                HttpResponseMessage response = await client.PostAsync($"https://www.ipqualityscore.com/api/json/url/7zZsro9PvWHQG64UX8nQGt61zZikoCAg/{url}", content);
                var responseString = await response.Content.ReadAsStringAsync();
                UrlScanner parsedResponse = JsonSerializer.Deserialize<UrlScanner>(responseString)!;
                
                if (parsedResponse.adult.Equals(true) || parsedResponse.@unsafe.Equals(true) || parsedResponse.suspicious.Equals(true))
                {
                    return Result<bool, string>.Err("link found to be unsafe and/or adult content. link was also found to be malicous.")!;
                }
                break;
            }
            
            return Result<bool, string>.Ok(true)!;
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

