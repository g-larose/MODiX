using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Servers;

namespace MODiX.Services.Interfaces
{
    public interface IGuildedApiProvider
    {
        Task<Server[]> GetMemberServers(string userId);
    }
}
