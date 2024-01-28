using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MODiX.Data.Config
{
    public class ModixDbContext: DbContext
    {
        public ModixDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
