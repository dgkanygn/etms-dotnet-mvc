using Microsoft.EntityFrameworkCore;
using ETTS.Models.Domain;

namespace ETTS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<OfferRequest> OfferRequests { get; set; }
        public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User -> CompanyProfile (1:1)
            modelBuilder.Entity<CompanyProfile>()
                .HasOne(cp => cp.User)
                .WithOne(u => u.CompanyProfile)
                .HasForeignKey<CompanyProfile>(cp => cp.UserId);

            // User -> OfferRequests (1:N)
            modelBuilder.Entity<OfferRequest>()
                .HasOne(or => or.CreatedByUser)
                .WithMany(u => u.CreatedOfferRequests)
                .HasForeignKey(or => or.CreatedByUserId);

            // OfferRequest -> Offers (1:N)
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.OfferRequest)
                .WithMany(or => or.Offers)
                .HasForeignKey(o => o.OfferRequestId);

            // User (Company) -> Offers (1:N)
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.CompanyUser)
                .WithMany(u => u.Offers)
                .HasForeignKey(o => o.CompanyUserId);

            // Enum'lari string olarak kaydet
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Offer>()
                .Property(o => o.Status)
                .HasConversion<string>();

            // Price icin decimal hassasiyeti
            modelBuilder.Entity<Offer>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);
        }
    }
}