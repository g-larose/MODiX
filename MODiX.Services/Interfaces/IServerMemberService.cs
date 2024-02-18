using Guilded;
using Guilded.Base;
using Guilded.Client;
using Guilded.Commands;
using Guilded.Servers;
using MODiX.Data.Models;

namespace MODiX.Services.Interfaces
{
    public interface IServerMemberService
    {
        Task<IList<MemberSummary>> GetServerMembersAsync(AbstractGuildedClient client, HashId serverId);
        Task<LocalServerMember> GetServerMemberAsync(HashId memberId);
        Task<string[]> GetMembersPermissions(Member member);
        Task<Server[]> GetMemberServersAsync(string userId);
        Task<bool> AddServerMemberToDBAsync(AbstractGuildedClient ctx, Member member);
        Task<bool> AddWarningAsync(HashId memberId);
        Task<bool> KickServerMemberAsync(HashId memberId);
        Task<bool> BanMemberAsync(HashId memberId);
        Task<bool> UnBanMemberAsync(HashId memberId);
        Task<bool> MuteMemberAsync(HashId memberId);
        Task<bool> UnMuteMemberAsync(HashId memberId);

    }
}
