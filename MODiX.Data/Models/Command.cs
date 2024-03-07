using Guilded.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Command
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Limit { get; set; }
        public int Cooldown { get; set; }
        public int Timeout { get; set; }
        public string? ServerId { get; set; }

    }
}
