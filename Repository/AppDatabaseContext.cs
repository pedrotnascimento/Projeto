using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{

    public class AppDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }

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
            modelBuilder.Entity<Message>();
            modelBuilder.Entity<ChatRoom>();
        }

    }
}