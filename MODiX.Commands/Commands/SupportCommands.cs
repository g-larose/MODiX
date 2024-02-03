using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Commands;

namespace MODiX.Commands.Commands
{
    public class SupportCommands : CommandModule
    {
        [Command(Aliases = new string[] { "support" })]
        [Description("creates a support ticket for mods to discuss")]
        public async Task Support(CommandEvent invokator, string[] title, string[] body)
        {

        }

        [Command(Aliases = new string[] { "set" })]
        [Description("set is designed to handle different commands. like [set] [prefix], [set] [supportchannel] ect.")]
        public async Task Set(CommandEvent invokator, string command, string context)
        {

        }
    }
}
