using Guilded.Commands;

namespace MODiX.Commands.Commands
{
    public class WikiCommands : CommandModule
    {
        [Command(Aliases = new string[] { "search" })]
        [Description("search wiki for information based on the query")]
        public async Task SearchWiki(CommandEvent invokator, [CommandParam] string query)
        {

        }
    }
}
