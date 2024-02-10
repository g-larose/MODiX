using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Guilded;
using Guilded.Base;
using Guilded.Servers;
using MODiX.Data;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.Interfaces;

namespace MODiX.Services.Services
{
    public class ServerMemberService : IServerMemberService, IDisposable
    {
        private ModixDbContextFactory _dbFactory = new();
        public async Task<bool> AddServerMemberToDBAsync(Member member)
        {
            using var db = _dbFactory.CreateDbContext();
            var localMember = db!.ServerMembers!.Where(x => x.UserId == member.Id.ToString());

            if (localMember is not null) return false;

            var newMember = new LocalServerMember
            {
                Id = Guid.NewGuid(),
                Nickname = member.Name,
                UserId = member.User.Id.ToString(),
                Xp = 0,
                ServerId = member.ServerId.ToString(),
                CreatedAt = member.CreatedAt,
                JoinedAt = DateTime.Now,
                RoleIds = member.RoleIds.ToList()
            };

            await db!.AddAsync(newMember);
            await db.SaveChangesAsync();
            return true;
            
        }

        public Task<bool> AddWarningAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BanMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetMembersPermissions()
        {
            throw new NotImplementedException();
        }

        public Task<LocalServerMember> GetServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<MemberSummary>> GetServerMembersAsync(GuildedBotClient client, HashId serverId)
        {
            var server = await client.GetServerAsync(serverId);
            var members = await client.GetMembersAsync(serverId);
            return members;
        }

        public Task<bool> KickServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<MemberSummary>> LoadServerMembersAsync(GuildedBotClient client, HashId serverId)
        {
            var server = await client.GetServerAsync(serverId);
            var members = await client.GetMembersAsync(serverId);
            return members;
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

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
