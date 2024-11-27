using Microsoft.EntityFrameworkCore;

using SkyHub.Models.Flight_Details;
using SkyHub.Models.Payment_Details;
using SkyHub.Models.Roles;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SkyHub.Data
{
    public class SkyHubDbContext : DbContext 
    {
        public SkyHubDbContext() { }
        public SkyHubDbContext(DbContextOptions<SkyHubDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=VJ-S\\SQLEXPRESS;Database=SkyHub1;Trusted_Connection=True;Trust Server Certificate=True");

            //base.OnConfiguring(optionsBuilder);

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<FlightOwner> FlightOwner { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<Flights> Flights { get; set; }
        public DbSet<SeatTypes> SeatTypes { get; set; }
        public DbSet<Seats> Seats { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<BookingItems> BookingItems { get; set; }
        public DbSet<BaggageInfos> BaggageInfos { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Refunds> Refunds { get; set; }


        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return storedHash == Convert.ToBase64String(hash);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // schemas
            modelBuilder.Entity<Users>().ToTable("Users", schema: "Roles");
            modelBuilder.Entity<Passenger>().ToTable("Passenger", schema: "Roles");
            modelBuilder.Entity<FlightOwner>().ToTable("FlightOwner", schema: "Roles");
            modelBuilder.Entity<Admin>().ToTable("Admin", schema: "Roles");
            modelBuilder.Entity<Routes>().ToTable("Routes", schema: "Flight_Details");
            modelBuilder.Entity<Flights>().ToTable("Flights", schema: "Flight_Details");
            modelBuilder.Entity<SeatTypes>().ToTable("SeatTypes", schema: "Flight_Details");
            modelBuilder.Entity<Seats>().ToTable("Seats", schema: "Flight_Details");
            modelBuilder.Entity<Bookings>().ToTable("Bookings", schema: "Flight_Details");
            modelBuilder.Entity<BookingItems>().ToTable("BookingItems", schema: "Flight_Details");
            modelBuilder.Entity<BaggageInfos>().ToTable("BaggageInfo", schema: "Flight_Details");
            modelBuilder.Entity<Payments>().ToTable("Payments", schema: "Payment_Details");
            modelBuilder.Entity<Refunds>().ToTable("Refunds", schema: "Payment_Details");


            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                // Email validation
                entity.HasAnnotation("Relational:CheckConstraint:CHK_Users_Email", "[Email] LIKE '%@%'");

                // Unique index for Email
                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.PasswordHash)
                    .IsRequired();

                // Password validation
                entity.HasAnnotation(
                    "Relational:CheckConstraint:CHK_Users_Password",
                    "[PasswordHash] LIKE '%[A-Z]%' AND " +
                    "[PasswordHash] LIKE '%[0-9]%' AND " +
                    "[PasswordHash] LIKE '%[^a-zA-Z0-9]%' AND " +
                    "LEN([PasswordHash]) >= 8"
                );

            });


            // Passenger-User (1:1)
            modelBuilder.Entity<Passenger>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Passenger>(c => c.UserId);

            // FlightOwner-User (1:1)
            modelBuilder.Entity<FlightOwner>()
                .HasOne(f => f.User)
                .WithOne(u => u.FlightOwner)
                .HasForeignKey<FlightOwner>(f => f.UserId);

            // Unique index for CompanyName
            modelBuilder.Entity<FlightOwner>()
                .HasIndex(f => f.CompanyName)
                .IsUnique();

            // Admin-User (1:1)
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId);


            // Routes-FlightOwner (Many-to-1)
            modelBuilder.Entity<Routes>()
                .HasOne(r => r.FlightOwner)
                .WithMany(o => o.Route)
                .HasForeignKey(r => r.FlightOwnerId);


            // Flights-FlightOwner (Many-to-1)
            modelBuilder.Entity<Flights>()
           .HasOne(f => f.FlightOwner)
           .WithMany()
           .HasForeignKey(f => f.FlightOwnerId)
           .OnDelete(DeleteBehavior.Cascade); // Cascade delete for FlightOwner

            // Configure the Route foreign key with NO ACTION on delete to prevent conflict
            modelBuilder.Entity<Flights>()
                .HasOne(f => f.Route)
                .WithMany(r => r.Flights)
                .HasForeignKey(f => f.RouteId)
                .OnDelete(DeleteBehavior.NoAction); // Restrict cascading delete

            // Flights-Seats (1-to-Many)
            modelBuilder.Entity<Seats>()
                .HasOne(s => s.Flight)
                .WithMany(f => f.Seats)
                .HasForeignKey(s => s.FlightId);

            // Unique index for Seat Number
            modelBuilder.Entity<Flights>()
                .HasIndex(e => e.FlightNumber)
                .IsUnique();


            // Seats-SeatTypes (Many-to-1)
            modelBuilder.Entity<Seats>()
                .HasOne(s => s.SeatType)
                .WithMany(st => st.Seats)
                .HasForeignKey(s => s.SeatTypeId);

            // Unique index for Seat Number
            modelBuilder.Entity<Seats>()
                .HasIndex(e => e.SeatNumber)
                .IsUnique();


            // Bookings-User (Many-to-1)
            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            // Bookings-Flight (Many-to-1)
            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.Flight)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FlightId);


            // BookingItems-Bookings (Many-to-1)
            modelBuilder.Entity<BookingItems>()
                .HasOne(bi => bi.Booking)
                .WithMany(b => b.BookingItems)
                .HasForeignKey(bi => bi.BookingId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // BookingItems-Seats (Many-to-1)
            modelBuilder.Entity<BookingItems>()
                .HasOne(bi => bi.Seat)
                .WithMany(s => s.BookingItems)
                .HasForeignKey(bi => bi.SeatId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete


            // BookingItems-SeatTypes (Many-to-1)
            modelBuilder.Entity<BookingItems>()
                .HasOne(bi => bi.SeatType)
                .WithMany(st => st.BookingItems)
                .HasForeignKey(bi => bi.SeatTypeId);


            // BaggageInfos-Flights(1-to-1)
            modelBuilder.Entity<BaggageInfos>()
                .HasOne(b => b.Flight)
                .WithOne(f => f.BaggageInfo)
                .HasForeignKey<BaggageInfos>(b => b.FlightId)
                .OnDelete(DeleteBehavior.Cascade);


            // Payments-Bookings (1-to-1)
            modelBuilder.Entity<Payments>()
            .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payments>(p => p.BookingId);

            // Unique index for TransactionId
            modelBuilder.Entity<Payments>()
                .HasIndex(f => f.TransactionId)
                .IsUnique();

            // Refunds-Payment (1-to-1)
            modelBuilder.Entity<Refunds>()
                .HasOne(r => r.Payment)
                .WithOne(p => p.Refund)
                .HasForeignKey<Refunds>(r => r.PaymentId);





            base.OnModelCreating(modelBuilder);

          
        }

        private static byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
