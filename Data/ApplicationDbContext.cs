using Microsoft.EntityFrameworkCore;
using VirocGanpati.Models;

namespace VirocGanpati.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Mandal> Mandals { get; set; }
        public DbSet<MandalArea> MandalAreas { get; set; }
        public DbSet<ArtiMorningTime> ArtiMorningTimes { get; set; }
        public DbSet<ArtiEveningTime> ArtiEveningTimes { get; set; }
        public DbSet<DateOfVisarjan> DateOfVisarjans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OtpMessage> OtpMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mandal>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Record>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<Document>().HasQueryFilter(d => !d.Record.IsDeleted);

            // RECORD ↔ MANDAL (Required FK)
            modelBuilder.Entity<Record>()
                .HasOne(r => r.Mandal)
                .WithMany()
                .HasForeignKey(r => r.MandalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Record>()
                .HasIndex(r => r.MandalId);

            modelBuilder.Entity<Mandal>()
                .HasOne(p => p.Updater)
                .WithMany()
                .HasForeignKey(p => p.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Record>()
                .HasOne(p => p.Updater)
                .WithMany()
                .HasForeignKey(p => p.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Mandal>()
              .HasIndex(p => p.MandalSlug)
              .IsUnique();

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Record)
                .WithMany(r => r.Documents)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Document>()
                .HasIndex(d => d.RecordId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Mandal)
                .WithMany()
                .HasForeignKey(u => u.MandalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Mobile)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.MandalId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.RoleId);

            // MANDAL ↔ AREA
            modelBuilder.Entity<Mandal>()
                .HasOne(m => m.Area)
                .WithMany()
                .HasForeignKey(m => m.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mandal>()
                .HasIndex(m => m.AreaId);

            // MANDAL ↔ ARTI MORNING TIME
            modelBuilder.Entity<Mandal>()
                .HasOne(m => m.ArtiMorningTime)
                .WithMany()
                .HasForeignKey(m => m.ArtiMorningTimeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mandal>()
                .HasIndex(m => m.ArtiMorningTimeId);

            // MANDAL ↔ ARTI EVENING TIME
            modelBuilder.Entity<Mandal>()
                .HasOne(m => m.ArtiEveningTime)
                .WithMany()
                .HasForeignKey(m => m.ArtiEveningTimeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mandal>()
                .HasIndex(m => m.ArtiEveningTimeId);

            // MANDAL ↔ DATE OF VISARJAN
            modelBuilder.Entity<Mandal>()
                .HasOne(m => m.DateOfVisarjan)
                .WithMany()
                .HasForeignKey(m => m.DateOfVisarjanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mandal>()
                .HasIndex(m => m.DateOfVisarjanId);


            // PAYMENT ↔ AREA
            modelBuilder.Entity<Payment>()
                .HasOne(m => m.Area)
                .WithMany()
                .HasForeignKey(m => m.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasIndex(m => m.AreaId);

            // PAYMENT ↔ ARTI MORNING TIME
            modelBuilder.Entity<Payment>()
                .HasOne(m => m.ArtiMorningTime)
                .WithMany()
                .HasForeignKey(m => m.ArtiMorningTimeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasIndex(m => m.ArtiMorningTimeId);

            // PAYMENT ↔ ARTI EVENING TIME
            modelBuilder.Entity<Payment>()
                .HasOne(m => m.ArtiEveningTime)
                .WithMany()
                .HasForeignKey(m => m.ArtiEveningTimeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasIndex(m => m.ArtiEveningTimeId);

            // PAYMENT ↔ DATE OF VISARJAN
            modelBuilder.Entity<Payment>()
                .HasOne(m => m.DateOfVisarjan)
                .WithMany()
                .HasForeignKey(m => m.DateOfVisarjanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasIndex(m => m.DateOfVisarjanId);


            base.OnModelCreating(modelBuilder);
        }
    }
}