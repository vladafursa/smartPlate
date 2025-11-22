using SmartPlate.DTOs.PlateOwnership;
using SmartPlate.Models;

namespace SmartPlate.Mappers
{
    public static class PlateOwnershipMapper
    {
        public static PlateOwnershipResponseDto ToDto(this PlateOwnershipRecord record)
        {
            return new PlateOwnershipResponseDto
            {
                Id = record.Id,
                PlateId = record.PlateId,
                PlateRegNumber = record.Plate.RegistrationNumber,
                OwnerId = record.OwnerId,
                OwnerUserName = record.Owner.UserName,
                Start = record.Start,
                End = record.End
            };
        }
    }
}
