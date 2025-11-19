using SmartPlate.Models.Enums;
namespace SmartPlate.Models;

public class Plate
{
    public Guid Id { get; set; }

    // Core identity
    public string RegistrationNumber { get; set; }
    public PlateType Type { get; set; }
    public List<PlateCategory> Categories { get; set; } = new();

    // Classification
    public string Region { get; set; }
    public int? YearIssued { get; set; }

    // Assignment / DVLA rules
    public bool CanApplyToAnyVehicle { get; set; }
    public bool IsAssigned { get; set; }
    public bool AvailableAsCertificate { get; set; }

    // Marketplace metadata
    public decimal Price { get; set; }
    public decimal TransferFee { get; set; } = 80m;
    public bool IsAuction { get; set; }

    public PlateSupplyType Supply { get; set; }

    // Ownership/trade state
    public Guid? CurrentOwnerId { get; set; }
    public PlateStatus Status { get; set; } = PlateStatus.Listed;

    // Tracking / value history
    public DateTime DateListed { get; set; }
    public List<PlateBid> BidHistory { get; set; }
}
