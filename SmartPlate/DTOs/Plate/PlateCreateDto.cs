using System.ComponentModel.DataAnnotations;
using SmartPlate.Models.Enums;
namespace SmartPlate.DTOs.Plate;

public class PlateCreateDto
{
    // Core identity
    [Required]
    [StringLength(15, MinimumLength = 1, ErrorMessage = "Registration number must be between 1 and 15 characters.")]
    public string RegistrationNumber { get; set; }
    [Required] public PlateType Type { get; set; }
    [Required] public List<PlateCategory> Categories { get; set; } = new();

    // Classification
    [Required]
    [StringLength(50, ErrorMessage = "Region name cannot exceed 50 characters.")]
    public string Region { get; set; }
    [Range(1900, 2025, ErrorMessage = "Year issued must be between 1900 and 2025.")]
    public int? YearIssued { get; set; }

    // DVLA rules
    public bool CanApplyToAnyVehicle { get; set; }
    public bool IsAssigned { get; set; }
    public bool AvailableAsCertificate { get; set; }

    // Marketplace metadata
    [Required]
    [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100,000.")]
    public decimal Price { get; set; }
    [Range(0, 1000, ErrorMessage = "Transfer fee must be between 0 and 1000.")]
    public decimal TransferFee { get; set; } = 80m;
    [Required]
    public bool IsAuction { get; set; }
    [Required]
    public PlateSupplyType Supply { get; set; }
    //status
    public PlateStatus Status { get; set; } = PlateStatus.Listed;
}
