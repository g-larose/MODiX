using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class ServerConfig
    {
        public Guid Id { get; set; }
        public string? ServerId { get; set; }
        public Guid DefaultChannel { get; set; }
        public string? Prefix { get; set; }


    }
}
