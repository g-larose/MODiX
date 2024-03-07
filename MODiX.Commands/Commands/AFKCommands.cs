using Guilded.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Commands.Commands
{
    public class AFKCommands: CommandModule
    {
        [Command(Aliases = [ "setafk" ])]
        [Description("set's a member to AFK")]
        public async Task Afk(CommandEvent invokator, TimeSpan duration)
        {

        }
    }
}
