using OnlineShop.Business.Services.Interfaces;
using OnlineShop.Common.DTOs;
using OnlineShop.DataAccess.EFModels;
using OnlineShop.DataAccess.EFModels.OnlineShop.DataAccess.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Business.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OnlineShopContext _context;
        public OrdersService(OnlineShopContext context) 
        {
            _context = context;
        }

        public async Task<int> CreateOrderFromCart(ShoppingCartDto shoppingCart, CheckoutRequestDto checkoutRequest, int authenticatedUserId)
        {
            var order = new Order
            {
                UserId = authenticatedUserId,
                PhoneNumber = checkoutRequest.PhoneNumber,
                Address = checkoutRequest.Address,
                FullName = checkoutRequest.FullName,
                OrderDate = DateTime.Now,
            };
            _context.Orders.Add(order);
            decimal totalOrderPrice = 0;

            await _context.SaveChangesAsync();
            var orderDetailsDto = new List<OrderDetailDto>();
            

            foreach (var item in shoppingCart.ShoppingCartItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    continue; 
                }

                product.StockQuantity -= item.Quantity;
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * item.Quantity
                };
                _context.OrderDetails.Add(orderDetail);
                totalOrderPrice += orderDetail.TotalPrice;
            }
            order.TotalOrderPrice = totalOrderPrice;
            await _context.SaveChangesAsync();

            return order.Id;
        }
        public async Task<OrderDto> GetOrderDetails(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderId} not found");
            }

            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address,
                FullName = order.FullName,
                OrderDate = order.OrderDate,
                TotalOrderPrice = order.TotalOrderPrice,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    TotalPrice = od.TotalPrice
                }).ToList()
            };

            return orderDto;
        }
    }
}

