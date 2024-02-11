using Guilded.Commands;

namespace MODiX.Commands.Commands
{
    public class ConfigCommands : CommandModule
    {
        #region UPDATE DEFAULT CHANNEL
        [Command(Aliases = new string[] { "updateDC", "udc" })]
        [Description("update the default channel")]
        public async Task UpdateDefaultChannel(CommandEvent invokator, string serverId, string channelId)
        {

        }
        #endregion
    }
}
