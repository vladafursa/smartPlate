using System.ComponentModel.DataAnnotations;
using SmartPlate.Models;
namespace SmartPlate.DTOs.PlateBid
{
    public record PlateBidCreateDto
    {
        public Guid PlateListingId { get; set; }
        public decimal Amount { get; set; }
    }
}
