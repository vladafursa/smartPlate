using SmartPlate.Models;
using SmartPlate.Models.Enums;
using SmartPlate.DTOs.Order;

namespace SmartPlate.Mappers
{
    public static class OrderMapper
    {
        public static OrderResponseDto ToDto(this Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                FinalPrice = order.FinalPrice,

                BuyerId = order.BuyerId,
                BuyerName = order.Buyer.UserName,
                BuyerEmail = order.Buyer.Email,

                SellerId = order.SellerId,
                SellerName = order.Seller.UserName,
                SellerEmail = order.Seller.Email,

                PlateListingId = order.PlateListingId,
                PlateId = order.PlateListing.Plate.Id,
                PlateRegistrationNumber = order.PlateListing.Plate.RegistrationNumber,

                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            };
        }
    }
}
