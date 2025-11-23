using System.ComponentModel.DataAnnotations;
using SmartPlate.Models.Enums;
namespace SmartPlate.DTOs.Order
{
    public class OrderFilterDto
    {
        public Guid? BuyerId { get; set; }
        public Guid? SellerId { get; set; }
        public Guid? PlateId { get; set; }
        public string? PlateRegistrationNumber { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}