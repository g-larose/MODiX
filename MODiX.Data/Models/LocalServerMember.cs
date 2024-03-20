using Guilded.Base;
using Guilded.Content;
using Guilded.Servers;
using Guilded.Users;
using MODiX.Data.Models;

namespace MODiX.Data.Models
{
    public class LocalServerMember
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? ServerId { get; set; }
        public long Xp { get; set; }
        public int Warnings { get; set; }
        public List<uint> RoleIds { get; set; } = new();
        public string? Nickname { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime JoinedAt { get; set; }
        public List<LocalChannelMessage>? Messages { get; set; }
        public List<string>? Nicknames { get; set; } = new();
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; } = new();
        public Bank Bank { get; set; } = null;
        public int BankId { get; set; }

    }
}
