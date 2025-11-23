using System.ComponentModel.DataAnnotations;
using SmartPlate.Models.Enums;

namespace SmartPlate.DTOs.PlateListing
{
    public class PlateListingFilterDto
    {
        // Plate filters
        public string? Search { get; set; }
        public PlateType? Type { get; set; }
        public List<PlateCategory>? Categories { get; set; }
        public string? Region { get; set; }
        public int? YearIssued { get; set; }
        public bool? IsAssigned { get; set; }
        public bool? CanApplyToAnyVehicle { get; set; }
        public bool? AvailableAsCertificate { get; set; }
        public PlateSupplyType? Supply { get; set; }

        // Listing filters
        public bool? IsAuction { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
