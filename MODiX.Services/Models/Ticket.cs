using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODiX.Services.Enums;

namespace MODiX.Services.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public TicketType TicketType { get; set; }
        public LocalServerMember Creator { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }


    }
}
