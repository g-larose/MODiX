using Guilded.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Backpack
    {
        public Guid Id { get; set; }
        public string? MemberId { get; set; }
        public ICollection<Item>? Items { get; set; }

    }
}
