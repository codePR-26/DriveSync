using Microsoft.EntityFrameworkCore;
using DriveSync.Models;

namespace DriveSync.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor used by Dependency Injection
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ================================
        // DATABASE TABLES (DbSets)
        // ================================

        // Account login table
        // Stores Owner / Admin / Customer users
        public DbSet<Account> Accounts { get; set; }

        // Vehicle table
        // Stores all vehicles available for booking
        public DbSet<Vehicle> Vehicles { get; set; }

        // Reservation table
        // Booking information created by customers
        public DbSet<Reservation> Reservations { get; set; }

        // Payment table
        // Payment history linked with reservations
        public DbSet<Payment> Payments { get; set; }


        // ================================
        // DATABASE CONFIGURATION RULES
        // ================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================================
            // ACCOUNT RULES
            // ================================

            // Make Email UNIQUE
            // Prevent duplicate accounts
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();


            // ================================
            // RESERVATION RELATIONSHIPS
            // ================================

            // Reservation → Customer (Account)
            // One Customer can have many Reservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(a => a.Reservations)
                .HasForeignKey(r => r.CustomerId)
                // Prevent accidental deletion of booking history
                .OnDelete(DeleteBehavior.Restrict);


            // Reservation → Vehicle
            // One Vehicle can have many Reservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Vehicle)
                .WithMany(v => v.Reservations)
                .HasForeignKey(r => r.VehicleId)
                // Vehicle deletion should not remove reservations
                .OnDelete(DeleteBehavior.Restrict);


            // Performance Index
            // Used for availability checking (overlapping booking search)
            modelBuilder.Entity<Reservation>()
                .HasIndex(r =>
                    new { r.VehicleId, r.StartDate, r.EndDate });


            // ================================
            // PAYMENT RELATIONSHIPS
            // ================================

            // Payment → Reservation
            // One Reservation can have multiple Payments
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Reservation)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ReservationId)
                // Protect payment history
                .OnDelete(DeleteBehavior.Restrict);


            // Payment lookup optimization
            // Faster fetching payments by reservation
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.ReservationId);
        }
    }
}