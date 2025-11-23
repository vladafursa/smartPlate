using SmartPlate.Models;
using SmartPlate.DTOs.PlateListing;

namespace SmartPlate.Mappers;

public static class PlateListingMapper
{
    public static PlateListingResponseDto ToDto(this PlateListing listing)
    {
        return new PlateListingResponseDto
        {

            Id = listing.Id,
            Price = listing.Price,
            TransferFee = listing.TransferFee,
            IsAuction = listing.IsAuction,
            Status = listing.Status.ToString(),
            DateListed = listing.DateListed,

            SellerId = listing.SellerId,
            SellerName = listing.Seller.UserName,
            SellerEmail = listing.Seller.Email,

            PlateId = listing.PlateId,
            PlateRegNumber = listing.Plate.RegistrationNumber,
            Type = listing.Plate.Type,
            Categories = listing.Plate.Categories.ToList(),
            Region = listing.Plate.Region,
            YearIssued = listing.Plate.YearIssued,
            CanApplyToAnyVehicle = listing.Plate.CanApplyToAnyVehicle,
            IsAssigned = listing.Plate.IsAssigned,
            AvailableAsCertificate = listing.Plate.AvailableAsCertificate,
            Supply = listing.Plate.Supply
        };
    }
}
