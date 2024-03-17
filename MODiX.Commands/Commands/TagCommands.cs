using Guilded.Base.Embeds;
using Guilded.Commands;
using MODiX.Services.Interfaces;
using MODiX.Services.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Commands.Commands
{
    public class TagCommands : CommandModule
    {
        private TagProviderService tagService = new();

        [Command(Aliases = new string[] { "tag" })]
        [Description("tag commands")]
        public async Task Tag(CommandEvent invokator, string cmd, string? title = null, string[]? args = null)
        {
            var embed = new Embed();
            if (cmd is not null)
            {
                switch (cmd)
                {
                    case "banhammer":
                        embed.SetThumbnail(new EmbedMedia("https://cdn.gilcdn.com/MediaChannelUpload/e615752d38a881e44543348ae8bda8c2-Full.webp?w=1500&h=1500"));
                        await invokator.CreateMessageAsync(embed);
                        break;
                    default:
                        embed.SetDescription("I couldn't fint the command, please add the command to the list of tag commands.");
                        embed.SetColor(EmbedColorService.GetColor("gray", Color.Gray));
                        await invokator.CreateMessageAsync(embed);
                        break;
                }
            }
            else
            {
                await tagService.HandleTagCommandAsync(cmd, args);
            }
            
        }
    }
}
