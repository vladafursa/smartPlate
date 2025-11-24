using System;
using System.Collections.Generic;
using SmartPlate.Models.Enums;

namespace SmartPlate.Models
{
    public class PlateListing
    {
        // Parameterless constructor for EF Core
        private PlateListing()
        {
        }

        // Private constructor 
        private PlateListing(Plate plate, User seller, decimal price, bool isAuction, decimal transferFee = 80m)
        {
            Id = Guid.NewGuid();
            Plate = plate ?? throw new ArgumentNullException(nameof(plate));
            PlateId = plate.Id;
            Seller = seller ?? throw new ArgumentNullException(nameof(seller));
            SellerId = seller.Id;

            Price = price;
            TransferFee = transferFee;
            IsAuction = isAuction;
            Status = PlateListingStatus.Listed;
            DateListed = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        // Associated plate
        public Guid PlateId { get; private set; }
        public Plate Plate { get; private set; } = null!;

        // Seller information
        public Guid SellerId { get; private set; }
        public User Seller { get; private set; } = null!;

        // Listing details
        public decimal Price { get; private set; }
        public decimal TransferFee { get; private set; }
        public bool IsAuction { get; private set; }
        public PlateListingStatus Status { get; private set; }
        public DateTime DateListed { get; private set; }


        // Factory method to create a new listing
        public static PlateListing Create(Plate plate, User seller, decimal price, bool isAuction, decimal transferFee = 80m)
        {
            return new PlateListing(plate, seller, price, isAuction, transferFee);
        }

        // Updates
        public void UpdatePrice(decimal newPrice)
        {
            Price = newPrice;
        }
        public void UpdateTransferFee(decimal newTransferFee)
        {
            TransferFee = newTransferFee;
        }
        public void UpdateIsAuction(bool newIsAuction)
        {
            IsAuction = newIsAuction;
        }

        // Changes in status
        public void MarkAsCompleted() => Status = PlateListingStatus.Sold;
        public void MarkAsPending() => Status = PlateListingStatus.PendingSale;
    }
}
