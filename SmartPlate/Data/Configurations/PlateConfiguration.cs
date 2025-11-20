using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartPlate.Models;
using SmartPlate.Models.Enums;

public class PlateConfiguration : IEntityTypeConfiguration<Plate>
{
    public void Configure(EntityTypeBuilder<Plate> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.RegistrationNumber).IsUnique();
        //conversion of enums
        builder.Property(p => p.Type)
                   .HasConversion<string>();

        builder.Property(p => p.Supply)
            .HasConversion<string>();

        builder.Property(p => p.Categories)
        .HasConversion(
        v => string.Join(',', v),
        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
              .Select(x => Enum.Parse<PlateCategory>(x))
              .ToList()
    );

    }
}
