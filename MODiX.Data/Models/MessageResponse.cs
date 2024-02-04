using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Users;

namespace MODiX.Data.Models
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public User? ReplyTo { get; set; }
        public string Response { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public bool IsPrivate { get; set; } = false;
        public bool IsSilent { get; set; } = false;
    }
}
