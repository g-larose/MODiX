using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Servers;
using MODiX.Services.Interfaces;

namespace MODiX.Services.Services
{
    public class GuildedApiService : IGuildedApiProvider
    {
        public Task<Server[]> GetMemberServers(string userId)
        {
            using var httpClient = new HttpClient();
            var endpoint = new Uri($"https://www.guilded.gg/api/v1/users/{userId}/servers");
            return null;
        }
    }
}
