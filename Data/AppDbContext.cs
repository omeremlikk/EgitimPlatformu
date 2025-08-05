using Microsoft.EntityFrameworkCore;
using EgitimPlatformu.Models;

namespace EgitimPlatformu.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Package> Packages { get; set; } = null!;
        public DbSet<UserPackage> UserPackages { get; set; } = null!;
        public DbSet<PackageRequest> PackageRequests { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasDefaultValue("Student");
            });

            // Message Configuration
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.SentMessages)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Receiver)
                      .WithMany(u => u.ReceivedMessages)
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

                    // Package Configuration
        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
        });

        // UserPackage Configuration
        modelBuilder.Entity<UserPackage>(entity =>
        {
            entity.HasOne(up => up.User)
                  .WithMany()
                  .HasForeignKey(up => up.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(up => up.Package)
                  .WithMany()
                  .HasForeignKey(up => up.PackageId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.UserId, e.PackageId }).IsUnique();
        });

        // PackageRequest Configuration
        modelBuilder.Entity<PackageRequest>(entity =>
        {
            entity.HasOne(pr => pr.User)
                  .WithMany()
                  .HasForeignKey(pr => pr.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pr => pr.Package)
                  .WithMany()
                  .HasForeignKey(pr => pr.PackageId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.UserId, e.PackageId }).IsUnique();
        });

        // Seed Data
        SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Admin User - Şifre: Admin123!
            var adminPasswordHash = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes("Admin123!" + "EgitimPlatformu2025")));
            
            // Student User - Şifre: Student123!  
            var studentPasswordHash = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes("Student123!" + "EgitimPlatformu2025")));

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@egitim.com",
                    FirstName = "Gurbet",
                    LastName = "CATIN",
                    PasswordHash = adminPasswordHash,
                    Role = "Teacher",
                    PhoneNumber = "5551234567",
                    CreatedAt = new DateTime(2025, 1, 1),
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    Email = "ogrenci@egitim.com",
                    FirstName = "Ahmet",
                    LastName = "YILMAZ",
                    PasswordHash = studentPasswordHash,
                    Role = "Student",
                    PhoneNumber = "5559876543",
                    CreatedAt = new DateTime(2025, 1, 1),
                                    IsActive = true
            }
        );

        // Package Seed Data
        modelBuilder.Entity<Package>().HasData(
            new Package
            {
                Id = 1,
                Name = "LGS",
                Description = "Lise Giriş Sınavı Hazırlık Paketi",
                Grade = "8. Sınıf",
                Price = 299.99m,
                VideoCount = 45,
                TestCount = 25,
                DurationMonths = 12,
                Features = "{\"matematik\":true,\"turkce\":true,\"fen\":true,\"sosyal\":true,\"ingilizce\":true}",
                IsActive = true,
                ColorCode = "#28a745",
                IconClass = "fas fa-school",
                CreatedAt = new DateTime(2025, 1, 1),
                UpdatedAt = new DateTime(2025, 1, 1)
            },
            new Package
            {
                Id = 2,
                Name = "TYT",
                Description = "Temel Yeterlilik Testi Hazırlık Paketi",
                Grade = "11-12. Sınıf",
                Price = 399.99m,
                VideoCount = 60,
                TestCount = 40,
                DurationMonths = 10,
                Features = "{\"matematik\":true,\"turkce\":true,\"fen\":true,\"sosyal\":true}",
                IsActive = true,
                ColorCode = "#007bff",
                IconClass = "fas fa-university",
                CreatedAt = new DateTime(2025, 1, 1),
                UpdatedAt = new DateTime(2025, 1, 1)
            },
            new Package
            {
                Id = 3,
                Name = "AYT",
                Description = "Alan Yeterlilik Testi Hazırlık Paketi",
                Grade = "12. Sınıf",
                Price = 499.99m,
                VideoCount = 75,
                TestCount = 50,
                DurationMonths = 8,
                Features = "{\"matematik\":true,\"fizik\":true,\"kimya\":true,\"biyoloji\":true}",
                IsActive = false, // Henüz aktif değil
                ColorCode = "#dc3545",
                IconClass = "fas fa-medal",
                CreatedAt = new DateTime(2025, 1, 1),
                UpdatedAt = new DateTime(2025, 1, 1)
            }
        );
        }
    }
}