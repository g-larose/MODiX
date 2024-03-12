using MODiX.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Log
    {
        public Guid Id { get; set; }
        public string? ServerId { get; set; }
        public string? ChannelId { get; set; }
        public string? Timestamp { get; set; }
        public string? Content { get; set; }
        public LogType Type { get; set; }

    }
}
