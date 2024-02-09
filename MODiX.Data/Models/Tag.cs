using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
        public string? Author { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
