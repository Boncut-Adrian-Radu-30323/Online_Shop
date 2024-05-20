using OnlineShop.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Business.Services.Interfaces;
using OnlineShop.Business.Services;

namespace OnlineShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartsService _shoppingCartsService;
        private readonly IOrdersService _ordersService;
        public ShoppingCartController(IShoppingCartsService service, IOrdersService ordersService)
        {
            _shoppingCartsService = service;
            _ordersService = ordersService;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartItemDto>>> GetShoppingCart(int userId)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId) || authenticatedUserId != userId)
            {
                return Unauthorized("User not authenticated or unauthorized");
            }

            var shoppingCart = await _shoppingCartsService.GetShoppingCart(userId);
            return Ok(shoppingCart);
        }
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateProductQuantity(int userId, [FromBody] ShoppingCartItemDto shoppingCartItemDto)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Unauthorized("User not authenticated");
            }

            await _shoppingCartsService.UpdateProductQuantity(shoppingCartItemDto, authenticatedUserId);

            return NoContent();
        }
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddProductToCart([FromBody]ShoppingCartItemDto shoppingCartItemDto)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("User not authenticated");
            }
            await _shoppingCartsService.AddProductToCart(shoppingCartItemDto, userId);

            return NoContent();
        }
        [HttpDelete("{userId}")]
        public async Task<ActionResult> RemoveProductFromCart(int userId, [FromBody] ShoppingCartItemDto shoppingCartItemDto)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId) || authenticatedUserId != userId)
            {
                return Unauthorized("User not authenticated or unauthorized");
            }

            shoppingCartItemDto.UserId = userId;
            await _shoppingCartsService.RemoveProductFromCart(shoppingCartItemDto);
            return NoContent();
        }
        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult> ClearCart(int userId)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId) || authenticatedUserId != userId)
            {
                return Unauthorized("User not authenticated or unauthorized");
            }

            await _shoppingCartsService.ClearCart(userId);
            return NoContent();
        }
        [HttpPost("checkout/{userId}")]
        public async Task<ActionResult> Checkout(int userId, [FromBody] CheckoutRequestDto checkoutRequest)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId) || authenticatedUserId != userId)
            {
                return Unauthorized("User not authenticated or unauthorized");
            }
            var shoppingCart = await _shoppingCartsService.GetShoppingCart(userId);
            int orderId = await _ordersService.CreateOrderFromCart(shoppingCart, checkoutRequest, userId);
            await _shoppingCartsService.ClearCart(userId);
            var orderDto = await _ordersService.GetOrderDetails(orderId);
            return Ok(new { Message = "Checkout successful!", OrderDetails = orderDto });
        }



    }
}