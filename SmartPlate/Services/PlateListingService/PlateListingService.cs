using SmartPlate.DTOs.PlateListing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartPlate.Models;
using SmartPlate.Models.Enums;
using SmartPlate.Mappers;
using Microsoft.EntityFrameworkCore;
using SmartPlate.Data;
using SmartPlate.Services.PlateOwnershipService;
namespace SmartPlate.Services.PlateListingService
{
    public class PlateListingService : IPlateListingService
    {
        private readonly AppDbContext _context;
        private readonly IPlateOwnershipService _ownershipService;

        public PlateListingService(AppDbContext context, IPlateOwnershipService ownershipService)
        {
            _context = context;
            _ownershipService = ownershipService;
        }

        public async Task<PlateListingResponseDto> CreateAsync(Guid CurrentUser, Guid PlateId, PlateListingCreateDto dto)
        {
            var plate = await _context.Plates.FindAsync(PlateId)
                ?? throw new Exception("Plate not found");

            var ownershipRecord = await _ownershipService.GetCurrentOwnershipRecordForPlateAsync(PlateId);

            if (ownershipRecord == null)
                throw new Exception("Plate does not have an owner.");

            if (ownershipRecord.OwnerId != CurrentUser)
                throw new Exception("Only the owner can create a listing.");

            var owner = await _context.Users.FindAsync(CurrentUser)
                ?? throw new Exception("Owner user not found");

            var listing = PlateListing.Create(plate, owner, dto.Price, dto.IsAuction);

            _context.PlateListings.Add(listing);
            await _context.SaveChangesAsync();

            return listing.ToDto();
        }

        public async Task<List<PlateListingResponseDto>> GetListingsAsync()
        {

            var listings = await _context.PlateListings
               .Include(l => l.Seller)
               .Include(l => l.Plate)
               .ToListAsync();
            return listings.Select(l => l.ToDto()).ToList();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var listing = await _context.PlateListings.FindAsync(id);

            if (listing == null) return false;

            _context.PlateListings.Remove(listing);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<PlateListingResponseDto> UpdateAsync(Guid listingId, PlateListingUpdateDto dto)
        {
            var listing = await _context.PlateListings
                .Include(l => l.Seller)
                .Include(l => l.Plate)
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (listing is null)
                throw new KeyNotFoundException($"Listing with ID {listingId} not found.");

            //  Apply changes only if values differ 
            if (listing.Price != dto.Price)
                listing.UpdatePrice(dto.Price);

            if (listing.TransferFee != dto.TransferFee)
                listing.UpdateTransferFee(dto.TransferFee);

            if (listing.IsAuction != dto.IsAuction)
                listing.UpdateIsAuction(dto.IsAuction);

            // Status change handling
            if (listing.Status != dto.Status)
            {
                switch (dto.Status)
                {
                    case PlateListingStatus.PendingSale:
                        listing.MarkAsPending();
                        break;

                    case PlateListingStatus.Sold:
                        listing.MarkAsCompleted();
                        break;

                    default:
                        break;
                }
            }

            await _context.SaveChangesAsync();

            return listing.ToDto();
        }

        public async Task<IEnumerable<PlateListingResponseDto>> GetFilteredAsync(PlateListingFilterDto filter)
        {
            var query = _context.PlateListings
                .Include(l => l.Plate)
                .Include(l => l.Seller)
                .AsQueryable();

            // Text search partial
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var text = filter.Search.ToLower();
                query = query.Where(l => l.Plate.RegistrationNumber.ToLower().Contains(text));
            }

            // Listing filters
            if (filter.IsAuction.HasValue)
                query = query.Where(l => l.IsAuction == filter.IsAuction);


            if (filter.MinPrice.HasValue)
                query = query.Where(l => l.Price >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                query = query.Where(l => l.Price <= filter.MaxPrice);

            // Plate filters
            if (filter.Type.HasValue)
                query = query.Where(l => l.Plate.Type == filter.Type);

            if (!string.IsNullOrWhiteSpace(filter.Region))
                query = query.Where(l => l.Plate.Region == filter.Region);

            if (filter.YearIssued.HasValue)
                query = query.Where(l => l.Plate.YearIssued == filter.YearIssued);

            if (filter.IsAssigned.HasValue)
                query = query.Where(l => l.Plate.IsAssigned == filter.IsAssigned);

            if (filter.CanApplyToAnyVehicle.HasValue)
                query = query.Where(l => l.Plate.CanApplyToAnyVehicle == filter.CanApplyToAnyVehicle);

            if (filter.AvailableAsCertificate.HasValue)
                query = query.Where(l => l.Plate.AvailableAsCertificate == filter.AvailableAsCertificate);

            if (filter.Supply.HasValue)
                query = query.Where(l => l.Plate.Supply == filter.Supply);


            var result = await query
                .OrderByDescending(l => l.DateListed)
                .ToListAsync();

            // Categories
            if (filter.Categories is { Count: > 0 })
            {
                result = result
                    .Where(l => l.Plate.Categories.Any(c => filter.Categories.Contains(c)))
                    .ToList();
            }



            return result.Select(x => x.ToDto());
        }
    }
}