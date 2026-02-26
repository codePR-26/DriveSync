using Microsoft.EntityFrameworkCore;
using DriveSync.Models;

namespace DriveSync.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       

        // Account login table
        public DbSet<Account> Accounts { get; set; }

        // Vehicle table
        public DbSet<Vehicle> Vehicles { get; set; }


        // Configure database rules
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Make Email UNIQUE
            // Prevent duplicate accounts
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();
        }
    }
}