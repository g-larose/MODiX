using Guilded;
using Guilded.Base;
using Guilded.Servers;
using MODiX.Services.Models;

namespace MODiX.Services.Interfaces
{
    public interface IServerMemberService
    {
        Task<List<Member>> GetServerMembersAsync(GuildedBotClient client, HashId serverId);
        Task<LocalServerMember> GetServerMemberAsync(HashId memberId);
        Task<bool> AddWarningAsync(HashId memberId);
        Task<bool> KickServerMemberAsync(HashId memberId);
        Task<bool> BanMemberAsync(HashId memberId);
        Task<bool> UnBanMemberAsync(HashId memberId);
        Task<bool> MuteMemberAsync(HashId memberId);
        Task<bool> UnMuteMemberAsync(HashId memberId);

    }
}
