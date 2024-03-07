using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MODiX.Data.Models;


namespace MODiX.Data
{
    public class ModixDbContext: DbContext
    {
        public DbSet<LocalServerMember>? ServerMembers { get; set; }
        public DbSet<Suggestion>? Suggestions { get; set; }
        public DbSet<Ticket>? Tickets { get; set; }
        public DbSet<LocalChannelMessage>? Messages { get; set; }
        public DbSet<Command>? Commands { get; set; }

        public ModixDbContext(DbContextOptions options) : base(options) 
        {
            //try
            //{
            //    Database.Migrate();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }

        public ModixDbContext()
        {
            //try
            //{
            //    Database.Migrate();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json"));
            var conStr = JsonSerializer.Deserialize<ConfigJson>(json);
            optionsBuilder.UseNpgsql(conStr!.ConnectionString);
        }

    }
}
