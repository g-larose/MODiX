using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Base;

namespace MODiX.Data.Models
{
    public class LocalChannelMessage
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public string? AuthorId { get; set; }
        public string? ServerId { get; set; }
        public string? MessageContent { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
