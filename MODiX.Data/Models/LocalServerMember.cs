﻿using Guilded.Base;
using Guilded.Content;
using Guilded.Servers;
using Guilded.Users;
using MODiX.Data.Models;

namespace MODiX.Data.Models
{
    public class LocalServerMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? UserId { get; set; }
        public string? ServerId { get; set; }
        public int Xp { get; set; }
        public int Warnings { get; set; }
        public int[] RoleIds { get; set; } = Array.Empty<int>();
        public string Nickname { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime JoinedAt { get; set; }
        public List<LocalChannelMessage>? Messages { get; set; }

    }
}