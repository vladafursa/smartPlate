using Microsoft.EntityFrameworkCore;
using SmartPlate.Models;

namespace SmartPlate.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Plate> Plates { get; set; }
        public DbSet<PlateListing> PlateListings { get; set; }
        public DbSet<PlateBid> PlateBids { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PlateOwnershipRecord> PlateOwnershipRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}

