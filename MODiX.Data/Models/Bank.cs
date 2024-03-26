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
        public int Id { get; set; }
        public Guid? Identifier { get; set; }
        public double AccountTotal { get; set; }
        public string? ServerId { get; set; }
        public string? DepositedAt { get; set; }
        public DateTimeOffset LastDaily { get; set; }
        public LocalServerMember ServerMember { get; set; }

    }
}
