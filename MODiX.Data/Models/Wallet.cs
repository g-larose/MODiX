using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public LocalServerMember? Member { get; set; }
        public string? MemberId { get; set; }
        public string? ServerId { get; set; }
        public int Points { get; set; }
    }
}
