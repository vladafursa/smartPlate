using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartPlate.Models;
public class PlateOwnershipRecordConfiguration : IEntityTypeConfiguration<PlateOwnershipRecord>
{
    public void Configure(EntityTypeBuilder<PlateOwnershipRecord> builder)
    {
        builder.HasKey(po => po.Id);

        // PlateOwnershipRecord - User
        builder.HasOne(po => po.Owner)
               .WithMany()
               .HasForeignKey(po => po.OwnerId)
               .OnDelete(DeleteBehavior.Restrict);

        // PlateOwnershipRecord - Plate
        builder.HasOne(po => po.Plate)
               .WithMany()
               .HasForeignKey(po => po.PlateId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(po => new { po.PlateId, po.OwnerId, po.Start }).IsUnique();
    }
}
