using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartPlate.Models;
public class PlateBidConfiguration : IEntityTypeConfiguration<PlateBid>
{
    public void Configure(EntityTypeBuilder<PlateBid> builder)
    {
        builder.HasKey(pb => pb.Id);

        // PlateBid - PlateListing  
        builder.HasOne<PlateListing>()
               .WithMany()
               .HasForeignKey(pb => pb.PlateListingId)
               .OnDelete(DeleteBehavior.Cascade);

        // PlateBid - User
        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(pb => pb.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pb => new { pb.PlateListingId, pb.UserId, pb.CreatedAt }).IsUnique();
    }
}
