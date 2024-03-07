using Guilded.Base.Embeds;
using Guilded.Commands;
using MODiX.Services.Interfaces;
using MODiX.Services.Services;
using System;
using System.Collections.Generic;
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
        public async Task Tag(CommandEvent invokator, string cmd, string title = null, string[] args = null)
        {
            var embed = new Embed();
            if (cmd is not null)
            {
                switch (cmd)
                {
                    case "banhammer":
                        await invokator.CreateMessageAsync("https://i.imgur.com/1oyBExo.gifv");
                        break;
                    default:
                        await invokator.CreateMessageAsync("un-recognizable command.");
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
