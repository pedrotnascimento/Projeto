using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{

    public class AppDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TimeAllocation> TimeAllocations { get; set; }
        public DbSet<TimeMoment> TimeMoments { get; set; }
        public DbSet<User> Users { get; set; }

        public string DbPath { get; }

        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ProjectDatabase.db");
        }

        // DATABASE IN MEMORY
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite($"Data Source=:memory:");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TimeMoment>();
            modelBuilder.Entity<TimeAllocation>();
        }

    }
}