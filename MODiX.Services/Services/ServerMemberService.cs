using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Guilded;
using Guilded.Base;
using Guilded.Servers;
using MODiX.Services.Interfaces;
using MODiX.Services.Models;

namespace MODiX.Services.Services
{
    public class ServerMemberService : IServerMemberService
    {
        public async Task<List<Member>> LoadServerMembersAsync(GuildedBotClient client, HashId serverId)
        {
            var server = await client.GetServerAsync(serverId);
            var members = await client.GetMembersAsync(serverId);
            return null;
        }
    }
}
