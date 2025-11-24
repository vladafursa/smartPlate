using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartPlate.Models;

public class PlateListingConfiguration : IEntityTypeConfiguration<PlateListing>
{
    public void Configure(EntityTypeBuilder<PlateListing> builder)
    {

        builder.HasKey(pl => pl.Id);

        // Enum conversion
        builder.Property(pl => pl.Status)
                   .HasConversion<string>();

        // PlateListings - Plate
        builder.HasOne<Plate>()
               .WithMany()
               .HasForeignKey(pl => pl.PlateId)
               .OnDelete(DeleteBehavior.Cascade);

        // PlateListings -  User
        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(pl => pl.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pl => new { pl.PlateId, pl.SellerId, pl.DateListed }).IsUnique();
    }
}
