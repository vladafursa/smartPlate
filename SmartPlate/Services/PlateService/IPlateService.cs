using Microsoft.EntityFrameworkCore;
using SmartPlate.Data;
using SmartPlate.DTOs.Plate;
using SmartPlate.Models;
using SmartPlate.Mappers;


namespace SmartPlate.Services.PlateService
{
    public interface IPlateService
    {
        Task<PlateResponseDto?> CreateAsync(PlateCreateDto dto);
        Task<PlateResponseDto?> GetByIdAsync(Guid id);
        Task<List<PlateResponseDto>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}