using Microsoft.EntityFrameworkCore;
using SmartPlate.Data;
using SmartPlate.DTOs.PlateOwnership;
using SmartPlate.Models;
using SmartPlate.Mappers;


namespace SmartPlate.Services.PlateOwnershipService
{
    public class PlateOwnershipService : IPlateOwnershipService
    {
        private readonly AppDbContext _context;

        public PlateOwnershipService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PlateOwnershipResponseDto> CreateAsync(Guid currentUserId, Guid PlateId)
        {
            var record = PlateOwnershipRecord.Create(
                await _context.Plates.FindAsync(PlateId) ?? throw new Exception("Plate not found"),
                await _context.Users.FindAsync(currentUserId) ?? throw new Exception("User not found"),
                DateTime.UtcNow
            );

            _context.PlateOwnershipRecords.Add(record);
            await _context.SaveChangesAsync();

            return record.ToDto();
        }

        public async Task<List<PlateOwnershipResponseDto>> GetAllOwnershipHistoryForPlateAsync(Guid plateId)
        {
            var records = await _context.PlateOwnershipRecords
                  .Where(r => r.PlateId == plateId)
                  .Include(r => r.Plate)
                  .Include(r => r.Owner)
                  .ToListAsync();

            return records.Select(r => r.ToDto()).ToList();
        }

        public async Task<List<PlateOwnershipResponseDto>> GetAllOwnershipHistoryForUserAsync(Guid userId)
        {
            var records = await _context.PlateOwnershipRecords
                .Where(r => r.OwnerId == userId)
                .Include(r => r.Owner)
                .Include(r => r.Plate)
                .ToListAsync();

            return records.Select(r => r.ToDto()).ToList();
        }

        public async Task<PlateOwnershipResponseDto> GetCurrentOwnershipRecordForPlateAsync(Guid plateId)
        {
            var record = await _context.PlateOwnershipRecords
                .Where(r => r.PlateId == plateId && r.End == null)
                .Include(r => r.Plate)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync();

            return record?.ToDto();
        }
        public async Task<List<PlateOwnershipResponseDto>> GetCurrentOwnershipRecordsForUserAsync(Guid userId)
        {
            var records = await _context.PlateOwnershipRecords
                .Where(r => r.OwnerId == userId && r.End == null)
                .Include(r => r.Owner)
                .Include(r => r.Plate)
                .ToListAsync();

            return records.Select(r => r.ToDto()).ToList();
        }
        public async Task<PlateOwnershipResponseDto> EndOwnershipAsync(Guid PlateId)
        {
            var record = await _context.PlateOwnershipRecords
                .Where(r => r.PlateId == PlateId && r.End == null)
                .Include(r => r.Plate)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync();

            if (record == null) return null;

            record.EndOwnership();
            await _context.SaveChangesAsync();

            return record.ToDto();
        }
        public async Task<bool> DeleteOwnershipRecordsAsync(Guid PlateId)
        {
            var records = await _context.PlateOwnershipRecords
                .Where(r => r.PlateId == PlateId)
                .ToListAsync();

            if (!records.Any()) return false;

            _context.PlateOwnershipRecords.RemoveRange(records);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<PlateOwnershipResponseDto> TransferOwnershipAsync(Guid PlateId, Guid newOwnerId)
        {
            var currentRecord = await _context.PlateOwnershipRecords
                .Where(r => r.PlateId == PlateId && r.End == null)
                .FirstOrDefaultAsync();

            if (currentRecord != null)
                currentRecord.EndOwnership();

            var newRecord = PlateOwnershipRecord.Create(
                plate: await _context.Plates.FindAsync(PlateId),
                owner: await _context.Users.FindAsync(newOwnerId),
                start: DateTime.UtcNow
            );

            _context.PlateOwnershipRecords.Add(newRecord);
            await _context.SaveChangesAsync();

            return newRecord.ToDto();
        }
    }
}