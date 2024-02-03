using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Guilded.Content;
using Microsoft.EntityFrameworkCore;
using MODiX.Config;
using MODiX.Services.Models;

namespace MODiX.Data.Config
{
    public class ModixDbContext: DbContext
    {
        public DbSet<LocalServerMember>? ServerMembers { get; set; }
        public DbSet<Suggestion>? Suggestions { get; set; }
        public DbSet<Ticket>? Tickets { get; set; }

        public ModixDbContext(DbContextOptions options) : base(options) {}
        public ModixDbContext() {}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json"));
            var conStr = JsonSerializer.Deserialize<ConfigJson>(json);
            optionsBuilder.UseNpgsql(conStr!.ConnectionString);
        }

    }
}
