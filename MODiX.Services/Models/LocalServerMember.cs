using Guilded.Users;

namespace MODiX.Services.Models
{
    public class LocalServerMember
    {
        public string Id { get; set; } = string.Empty;
        public User? User { get; set; }
        public int Xp { get; set; }
        public int Warnings { get; set; }
        public int[] RoleIds { get; set; } = Array.Empty<int>();
        public string Nickname { get; set; } = string.Empty;
        public string JoinedAt { get; set; } = string.Empty;
        public bool IsOwner { get; set; } = false;
        public string[] Servers { get; set; } = Array.Empty<string>();

    }
}
