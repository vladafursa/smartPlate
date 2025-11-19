namespace SmartPlate.Models;

public class PlateOwnershipRecord
{
    public Guid Id { get; set; }
    public Guid PlateId { get; set; }
    public Guid OwnerId { get; set; }

    public DateTime OwnedFrom { get; set; }
    public DateTime? OwnedTo { get; set; } // null = still owns
}
