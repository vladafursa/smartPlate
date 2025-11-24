using SmartPlate.Models.Enums;

namespace SmartPlate.Models;

public class Order
{
    private Order() { }

    private Order(PlateListing listing, User buyer)
    {
        Id = Guid.NewGuid();
        PlateListing = listing;
        PlateListingId = listing.Id;

        Buyer = buyer;
        BuyerId = buyer.Id;

        SellerId = listing.SellerId;
        Seller = listing.Seller;

        FinalPrice = listing.Price;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public decimal FinalPrice { get; private set; }

    // Seller reference 
    public Guid SellerId { get; private set; }
    public User Seller { get; private set; } = null!;

    // Buyer reference
    public Guid BuyerId { get; private set; }
    public User Buyer { get; private set; } = null!;

    // Listing purchased
    public Guid PlateListingId { get; private set; }
    public PlateListing PlateListing { get; private set; } = null!;

    // Lifecycle state
    public OrderStatus Status { get; private set; }

    // Timestamp of purchase creation
    public DateTime CreatedAt { get; private set; }

    // Factory method to enforce valid creation
    public static Order Create(PlateListing listing, User buyer)
    {
        return new Order(listing, buyer);
    }
    public void MarkCompleted()
    {
        Status = OrderStatus.Completed;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }
}
