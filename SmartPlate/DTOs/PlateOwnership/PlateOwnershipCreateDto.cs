using System;

namespace SmartPlate.DTOs.PlateOwnership
{
    public class PlateOwnershipCreateDto
    {
        public Guid PlateId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime Start { get; set; }
    }
}
