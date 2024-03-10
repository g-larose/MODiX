using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.Json;
using Guilded;
using Guilded.Base;
using Guilded.Client;
using Guilded.Commands;
using Guilded.Servers;
using Microsoft.Win32.SafeHandles;
using MODiX.Data;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.BaseModules;
using MODiX.Services.Interfaces;

namespace MODiX.Services.Services
{
    public class ServerMemberService : DisposableBase, IServerMemberService, IDisposable
    {
        private ModixDbContextFactory _dbFactory = new();
        
        public async Task<Result<Member, string>> AddServerMemberToDBAsync(AbstractGuildedClient ctx, Member member)
        {
            var serverId = member.ServerId;
            var memberId = member.Id;
            var user = await ctx.GetMemberAsync((HashId)serverId!, memberId);
            var xp = await ctx.AddXpAsync((HashId)serverId!, memberId, 0);
            var roles = await ctx.GetMemberRolesAsync((HashId)serverId!, memberId);
            using var db = _dbFactory.CreateDbContext();
            var localMember = db!.ServerMembers!.Where(x => x.UserId == member.Id.ToString() && x.ServerId!.Equals(member.ServerId.ToString()));

            if (localMember.Count() > 0) return "error: member already exists in the database.";

            var newMember = new LocalServerMember
            {
                Id = Guid.NewGuid(),
                Nickname = member.Name,
                UserId = member.User.Id.ToString(),
                Xp = xp,
                ServerId = member.ServerId.ToString(),
                CreatedAt = member.CreatedAt,
                JoinedAt = user.JoinedAt,
                RoleIds = [.. roles],
                Wallet = new Wallet()
                {
                    Id = Guid.NewGuid(),
                    MemberId = member.Id.ToString(),
                    Points = 0
                },
            };

            await db!.AddAsync(newMember);
            await db.SaveChangesAsync();
            return member;
            
        }

        public async Task<Result<Member, string>> AddWarningAsync(string memberId)
        {
            try
            {
                var db = _dbFactory.CreateDbContext();
                var member = db.ServerMembers!.Where(x => x.UserId == memberId).FirstOrDefault();
                if (member is not null)
                {
                    var warnings = member.Warnings += 1;
                    member.Warnings = warnings;
                    db.Update(member);
                    await db.SaveChangesAsync();
                    return "success";
                }
                else
                {
                    var localMember = new LocalServerMember();

                }
                   
            }
            catch(Exception e)
            {
                return $"failure: {e.Message}";
            }
             return "member not found";
            
        }

        public Task<bool> BanMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public static new void Dispose()
        {
            DisposableBase disposableBase = new();
            disposableBase.Dispose();
        }

        public Task<string[]> GetMembersPermissions(Member member)
        {
            return null;
              
        }

        public Task<LocalServerMember> GetServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<MemberSummary>> GetServerMembersAsync(AbstractGuildedClient client, HashId serverId)
        {
            var server = await client.GetServerAsync(serverId);
            var members = await client.GetMembersAsync(serverId);
            return members;
        }

        public Task<bool> KickServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<MemberSummary>> LoadServerMembersAsync(AbstractGuildedClient client, HashId serverId)
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

        public async Task<Server[]> GetMemberServersAsync(string userId)
        {
            using var httpClient = new HttpClient();
           // var endpoint = new Uri($"https://www.guilded.gg/api/v1/users/{userId}/servers");
            var serverEndpoint = new Uri($"https://www.guilded.gg/api/v1/servers/{userId}/members");
            var jsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json");
            var json = await File.ReadAllTextAsync(jsonFile);
            var token = JsonSerializer.Deserialize<ConfigJson>(json!)!.Token!;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            
            var response = await httpClient.GetStringAsync(serverEndpoint);
            
            return null; ;

        }
    }
}
