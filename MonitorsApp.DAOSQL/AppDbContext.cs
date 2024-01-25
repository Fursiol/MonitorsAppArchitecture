using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorsApp.DAOSQL
{
    public class AppDbContext : DbContext
    {
        public DbSet<EfCoreProducer> Producers { get; set; }
        public DbSet<EfCoreMonitor> Monitors { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=monitor.db");
            }
        }
    }
}
