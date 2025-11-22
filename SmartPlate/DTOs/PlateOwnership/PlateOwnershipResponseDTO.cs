using System;

namespace SmartPlate.DTOs.PlateOwnership
{
    public class PlateOwnershipResponseDto
    {
        public Guid Id { get; set; }
        public Guid PlateId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}
