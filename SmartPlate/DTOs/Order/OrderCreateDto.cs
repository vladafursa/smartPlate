using System.ComponentModel.DataAnnotations;
using SmartPlate.Models.Enums;
namespace SmartPlate.DTOs.Order
{
    public record class OrderCreateDto
    {
        public Guid PlateListingId { get; set; }
    }
}