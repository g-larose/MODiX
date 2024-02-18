using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string? BackpackId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
