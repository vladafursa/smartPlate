using System.ComponentModel.DataAnnotations;
using SmartPlate.Models.Enums;
namespace SmartPlate.DTOs.Order
{
    public record class OrderResponseDto
    {
        public Guid Id { get; set; }
        public decimal FinalPrice { get; set; }

        //buyer details
        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; } = null!;
        public string BuyerEmail { get; set; } = null!;

        //seller details
        public Guid SellerId { get; set; }
        public string SellerName { get; set; } = null!;
        public string SellerEmail { get; set; } = null!;

        //plate details
        public Guid PlateListingId { get; set; }
        public Guid PlateId { get; set; }
        public string PlateRegistrationNumber { get; set; } = null!;

        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}