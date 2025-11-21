using System.ComponentModel.DataAnnotations;
using SmartPlate.Models.Enums;

namespace SmartPlate.DTOs.Plate
{
    public class PlateResponseDto
    {
        public Guid Id { get; set; }

        // Core identity
        public string RegistrationNumber { get; set; } = default!;
        public PlateType Type { get; set; }
        public List<PlateCategory> Categories { get; set; } = new();

        // Classification
        public string Region { get; set; } = default!;
        public int? YearIssued { get; set; }

        // DVLA rules
        public bool CanApplyToAnyVehicle { get; set; }
        public bool IsAssigned { get; set; }
        public bool AvailableAsCertificate { get; set; }

        public PlateSupplyType Supply { get; set; }
    }
}