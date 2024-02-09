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
            if (title is not null)
            {

            }
            else
            {
                await tagService.HandleTagCommandAsync(cmd, args);
            }
            
        }
    }
}
