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
        public Task<bool> AddWarningAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BanMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<LocalServerMember> GetServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Member>> GetServerMembersAsync(GuildedBotClient client, HashId serverId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> KickServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Member>> LoadServerMembersAsync(GuildedBotClient client, HashId serverId)
        {
            var server = await client.GetServerAsync(serverId);
            var members = await client.GetMembersAsync(serverId);
            return null;
        }

        public Task<bool> MuteMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnBanMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnMuteMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }
    }
}
