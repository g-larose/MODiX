using MODiX.Data.Enums;


namespace MODiX.Data.Models
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
