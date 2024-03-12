using Guilded.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Bank
    {
        public Guid Id { get; set; }
        public double AccountTotal { get; set; }
        public string? ServerId { get; set; }
        public Guid? MemberId { get; set; }
    }
}
