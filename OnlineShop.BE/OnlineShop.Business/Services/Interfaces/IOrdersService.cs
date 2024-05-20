using OnlineShop.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Business.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<int>CreateOrderFromCart(ShoppingCartDto shoppingCart, CheckoutRequestDto checkoutRequest, int authenticatedUserId);
        Task<OrderDto> GetOrderDetails(int orderId);
    }
}
