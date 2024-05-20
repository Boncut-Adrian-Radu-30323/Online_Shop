using OnlineShop.Common.DTOs;
using OnlineShop.DataAccess.EFModels;
namespace OnlineShop.Business.Services.Interfaces
{
    public interface IShoppingCartsService
    {
        Task AddProductToCart(ShoppingCartItemDto shoppingCartItemDto, int authenticatedUserId);
        Task<ShoppingCartItemDto> UpdateProductQuantity(ShoppingCartItemDto shoppingCartItemDto, int authenticatedUserId);
        Task RemoveProductFromCart(ShoppingCartItemDto shoppingCartItemDto);
        Task ClearCart(int userId);
        Task<ShoppingCartDto> GetShoppingCart(int userId);
        Task<User> GetUserWithCart(int userId);
    }
}
