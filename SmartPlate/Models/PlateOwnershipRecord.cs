using System;

namespace SmartPlate.Models
{
    public class PlateOwnershipRecord
    {
        // Parameterless constructor for EF Core
        private PlateOwnershipRecord() { }

        // Private constructor to initialize ownership
        private PlateOwnershipRecord(Plate plate, User owner, DateTime start)
        {
            Id = Guid.NewGuid();
            Plate = plate ?? throw new ArgumentNullException(nameof(plate));
            PlateId = plate.Id;
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            OwnerId = owner.Id;
            Start = start;
            End = null; // Ownership starts as active
        }

        public Guid Id { get; private set; }

        // Associated plate
        public Guid PlateId { get; private set; }
        public Plate Plate { get; private set; } = null!;

        // Owner information
        public Guid OwnerId { get; private set; }
        public User Owner { get; private set; } = null!;

        // Ownership period
        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; }

        // Factory method to create a new ownership record
        public static PlateOwnershipRecord Create(Plate plate, User owner, DateTime start)
        {
            return new PlateOwnershipRecord(plate, owner, start);
        }

        // Mark the end of ownership
        public void EndOwnership()
        {
            End = DateTime.UtcNow;
        }

        // Check if the ownership is currently active
        public bool IsActive() => End == null;
    }
}
