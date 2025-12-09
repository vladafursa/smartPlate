using System.ComponentModel.DataAnnotations;
using SmartPlate.Models;
namespace SmartPlate.DTOs.PlateBid
{
    public record PlateListingCreateDto
    {
        public decimal Amount { get; set; }
    }
}
