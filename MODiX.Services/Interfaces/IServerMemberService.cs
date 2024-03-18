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
        Task<Result<IList<MemberSummary>, string>> GetServerMembersAsync(HashId serverId);
        Task<Result<LocalServerMember, string>> GetServerMemberAsync(HashId memberId);
        Task<Result<string[], string>> GetMembersPermissionsAsync(Member member);
        Task<Result<Role, string>> GetMemberRoleNameAsync(Member member, uint roleId);
        Task<Server[]> GetMemberServersAsync(string userId);
        Task<Result<Member, string>> AddServerMemberToDBAsync(Member member);
        Task<Result<LocalServerMember, string>> AddWarningAsync(string memberId);
        Task<bool> KickServerMemberAsync(HashId memberId);
        Task<bool> BanMemberAsync(HashId memberId);
        Task<bool> UnBanMemberAsync(HashId memberId);
        Task<bool> MuteMemberAsync(HashId memberId);
        Task<bool> UnMuteMemberAsync(HashId memberId);


    }
}
