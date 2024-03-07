using Guilded.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class DiceRoller
    {
        public Guid Id { get; set; }
        public Member? Member { get; set; }
        public List<int> Die { get; set; } = new();
        public int Sides { get; set; }
        public string? RolledAt { get; set; }
        public bool IsValid { get; set; }

    }
}
