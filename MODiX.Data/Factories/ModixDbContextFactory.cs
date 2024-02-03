using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MODiX.Config;
using MODiX.Data.Config;

namespace MODiX.Data.Factories
{
    public class ModixDbContextFactory : IDesignTimeDbContextFactory<ModixDbContext>
    {
        public ModixDbContext CreateDbContext(string[] args)
        {
            var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json"));
            var conStr = JsonSerializer.Deserialize<ConfigJson>(json)!.ConnectionString;
            var options = new DbContextOptionsBuilder<ModixDbContext>();
            options.UseNpgsql(conStr);

            return new ModixDbContext(options.Options);

        }
    }
}
