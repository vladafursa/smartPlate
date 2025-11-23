using SmartPlate.DTOs.PlateListing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SmartPlate.Services.PlateListingService
{

    public interface IPlateListingService
    {
        Task<PlateListingResponseDto> CreateAsync(Guid SellerId, Guid PlateId, PlateListingCreateDto dto);
        Task<List<PlateListingResponseDto>> GetListingsAsync();
        Task<PlateListingResponseDto> UpdateAsync(Guid listingId, PlateListingUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<PlateListingResponseDto>> GetFilteredAsync(PlateListingFilterDto filter);
    }
}
