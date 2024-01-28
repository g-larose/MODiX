using Guilded;
using Guilded.Base;
using Guilded.Servers;

namespace MODiX.Services.Interfaces
{
    public interface IServerMemberService
    {
        Task<List<Member>> LoadServerMembersAsync(GuildedBotClient client, HashId serverId);
    }
}
