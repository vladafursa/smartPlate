
using System.ComponentModel.DataAnnotations;
using SmartPlate.Models.Enums;
namespace SmartPlate.DTOs.Plate;

public class PlateCreateDto
{
    // Core identity
    [Required]
    [StringLength(15, MinimumLength = 1, ErrorMessage = "Registration number must be between 1 and 15 characters.")]
    public required string RegistrationNumber { get; set; }
    [Required] public required PlateType Type { get; set; }
    [Required] public required List<PlateCategory> Categories { get; set; } = new();

    // Classification
    [Required]
    [StringLength(50, ErrorMessage = "Region name cannot exceed 50 characters.")]
    public required string Region { get; set; }
    [Range(1900, 2025, ErrorMessage = "Year issued must be between 1900 and 2025.")]
    public required int? YearIssued { get; set; }

    // DVLA rules
    public bool CanApplyToAnyVehicle { get; set; }
    public bool IsAssigned { get; set; }
    public bool AvailableAsCertificate { get; set; }

    [Required]
    public required PlateSupplyType Supply { get; set; }
}


