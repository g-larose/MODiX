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
        private AbstractGuildedClient _client { get; set; }

        public ServerMemberService(AbstractGuildedClient client)
        {
            _client = client;
        }

        #region ADD SERVER MEMBER TO DB
        public async Task<Result<Member, string>> AddServerMemberToDBAsync(Member member)
        {
            var serverId = member.ServerId;
            var memberId = member.Id;
            var user = await _client.GetMemberAsync((HashId)serverId!, memberId);
            if (user.IsBot) return Result<Member, string>.Err("isBot")!;

            var xp = await _client.AddXpAsync((HashId)serverId!, memberId, 0);
            var roles = await _client.GetMemberRolesAsync((HashId)serverId!, memberId);
            
            using var db = _dbFactory.CreateDbContext();
            var localMember = db!.ServerMembers!.Where(x => x.UserId == member.Id.ToString()).FirstOrDefault();

            if (localMember is not null) return Result<Member, string>.Err("failure: member already exists in database.")!;
           
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
                    MemberId = member.Id.ToString(),
                    ServerId = serverId.ToString(),
                    Points = 0
                }
            };

            await db!.AddAsync(newMember);
            await db.SaveChangesAsync();
            return member;
            
        }
        #endregion

        #region ADD WARNING TO MEMBER
        public async Task<Result<LocalServerMember, string>> AddWarningAsync(string memberId)
        {
            try
            {
                using var db = _dbFactory.CreateDbContext();
                var member = db.ServerMembers!.Where(x => x.UserId == memberId).FirstOrDefault();
                if (member is not null)
                {
                    var warnings = member.Warnings += 1;
                    member.Warnings = warnings;
                    db.Update(member);
                    await db.SaveChangesAsync();
                    return Result<LocalServerMember, string>.Ok(member)!;
                }
                else
                {
                    return Result<LocalServerMember, string>.Err("failuer: could't find member in database")!;
                }
                   
            }
            catch(Exception e)
            {
                return Result<LocalServerMember, string>.Err($"{e.Message}")!;
            }
            
        }
        #endregion

        public Task<bool> BanMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public static new void Dispose()
        {
            DisposableBase disposableBase = new();
            disposableBase.Dispose();
        }

        public Task<Result<string[], string>> GetMembersPermissionsAsync(Member member)
        {
            return null;
              
        }

        public Task<Result<LocalServerMember, string>> GetServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IList<MemberSummary>, string>> GetServerMembersAsync(HashId serverId)
        {
            var members = await _client.GetMembersAsync(serverId);
            return members.ToList();
        }

        public Task<bool> KickServerMemberAsync(HashId memberId)
        {
            throw new NotImplementedException();
        }

        #region LOAD LOCAL SERVER MEMBERS
        /// <summary>
        /// Loads server members from database
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns>List of LocalServerMember</LocalServerMember></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Result<List<LocalServerMember>, string> LoadServerMembers(HashId serverId)
        {

            throw new NotImplementedException();

        }
        #endregion

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

        public async Task<Result<Role, string>> GetMemberRoleNameAsync(Member member, uint roleId)
        {
            var serverId = member.ServerId;
            var server = await member.ParentClient.GetServerAsync((HashId)serverId!);
            var role = await server.ParentClient.GetRoleAsync(serverId, roleId);
            if (role is null) return Result<Role, string>.Err("failure")!;
            return role;
        }
    }
}
