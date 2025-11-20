using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartPlate.Models;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {

        builder.HasKey(o => o.Id);

        //conversion of enums
        builder.Property(o => o.Status)
                   .HasConversion<string>();

        // Order - PlateListing
        builder.HasOne<PlateListing>()
               .WithMany()
               .HasForeignKey(o => o.PlateListingId)
               .OnDelete(DeleteBehavior.Restrict);

        // Orders - User
        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(o => o.BuyerId)
               .OnDelete(DeleteBehavior.Restrict);

        // Orders - User
        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(o => o.SellerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => new { o.PlateListingId, o.BuyerId, o.SellerId, o.CreatedAt }).IsUnique();
    }
}
