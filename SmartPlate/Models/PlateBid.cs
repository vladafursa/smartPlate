namespace SmartPlate.Models;

public class PlateBid
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid UserId { get; set; }
}
