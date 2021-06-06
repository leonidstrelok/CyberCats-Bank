using ActivityLogger.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityLogger.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}