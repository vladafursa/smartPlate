using SmartPlate.DTOs.PlateOwnership;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SmartPlate.Services.PlateOwnershipService
{

    public interface IPlateOwnershipService
    {
        Task<PlateOwnershipResponseDto> CreateAsync(Guid currentUserId, Guid PlateId);
        Task<List<PlateOwnershipResponseDto>> GetAllOwnershipHistoryForPlateAsync(Guid plateId);
        Task<List<PlateOwnershipResponseDto>> GetAllOwnershipHistoryForUserAsync(Guid userId);
        Task<PlateOwnershipResponseDto> GetCurrentOwnershipRecordForPlateAsync(Guid PlateId);
        Task<List<PlateOwnershipResponseDto>> GetCurrentOwnershipRecordsForUserAsync(Guid userId);
        Task<PlateOwnershipResponseDto> EndOwnershipAsync(Guid PlateId);
        Task<bool> DeleteOwnershipRecordsAsync(Guid PlateId);
        Task<PlateOwnershipResponseDto> TransferOwnershipAsync(Guid PlateId, Guid newOwnerId);
    }
}
