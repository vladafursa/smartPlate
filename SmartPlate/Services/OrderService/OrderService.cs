using Microsoft.EntityFrameworkCore;
using SmartPlate.Data;
using SmartPlate.Models;
using SmartPlate.DTOs.Order;
using SmartPlate.Mappers;
using SmartPlate.Services.PlateOwnershipService;
namespace SmartPlate.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IPlateOwnershipService _ownershipService;

        public OrderService(AppDbContext context, IPlateOwnershipService ownershipService)
        {
            _context = context;
            _ownershipService = ownershipService;
        }

        // Create order
        public async Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto dto, Guid buyerId)
        {
            var listing = await _context.PlateListings
                .Include(l => l.Plate)
                .Include(l => l.Seller)
                .FirstOrDefaultAsync(l => l.Id == dto.PlateListingId);

            if (listing == null)
                throw new Exception("Listing not found");

            var buyer = await _context.Users.FindAsync(buyerId);
            if (buyer == null)
                throw new Exception("Buyer not found");

            var order = Order.Create(listing, buyer);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.ToDto();
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.PlateListing)
                    .ThenInclude(l => l.Plate)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(o => o.ToDto()).ToList();
        }


        // Filter orders
        public async Task<IEnumerable<OrderResponseDto>> FilterOrdersAsync(OrderFilterDto filter)
        {
            var query = _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.PlateListing)
                    .ThenInclude(l => l.Plate)
                .AsQueryable();

            if (filter.BuyerId.HasValue)
                query = query.Where(o => o.BuyerId == filter.BuyerId);

            if (filter.SellerId.HasValue)
                query = query.Where(o => o.SellerId == filter.SellerId);

            if (filter.PlateId.HasValue)
                query = query.Where(o => o.PlateListing.PlateId == filter.PlateId.Value);

            if (!string.IsNullOrWhiteSpace(filter.PlateRegistrationNumber))
                query = query.Where(o => o.PlateListing.Plate.RegistrationNumber == filter.PlateRegistrationNumber);

            if (filter.Status.HasValue)
                query = query.Where(o => o.Status == filter.Status.Value);

            if (filter.CreatedAfter.HasValue)
                query = query.Where(o => o.CreatedAt >= filter.CreatedAfter.Value);

            if (filter.CreatedBefore.HasValue)
                query = query.Where(o => o.CreatedAt <= filter.CreatedBefore.Value);

            var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();

            return orders.Select(o => o.ToDto());
        }

        // Change status
        public async Task<OrderResponseDto> CompleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.PlateListing)
                    .ThenInclude(l => l.Plate)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            await _ownershipService.TransferOwnershipAsync(
                   order.PlateListing.PlateId,
                   order.BuyerId
               );

            order.MarkCompleted();
            await _context.SaveChangesAsync();

            return order.ToDto();
        }

        public async Task<OrderResponseDto> CancelOrderAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.PlateListing)
                    .ThenInclude(l => l.Plate)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            order.Cancel();
            await _context.SaveChangesAsync();

            return order.ToDto();
        }
    }
}

