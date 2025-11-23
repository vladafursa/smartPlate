using SmartPlate.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SmartPlate.Services.OrderService
{

    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto dto, Guid buyerId);
        Task<List<OrderResponseDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderResponseDto>> FilterOrdersAsync(OrderFilterDto filter);
        Task<OrderResponseDto> CompleteOrderAsync(Guid orderId);
        Task<OrderResponseDto> CancelOrderAsync(Guid orderId);
    }
}
