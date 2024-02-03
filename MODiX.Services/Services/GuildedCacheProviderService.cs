using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Base;
using Guilded.Servers;

namespace MODiX.Services.Services
{
    public class GuildedCacheProviderService
    {
        public Dictionary<HashId, Member> memberCache = new();
        public Dictionary<HashId, IChannel> channelCache = new();

    }
}
