using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Business.Services.Interfaces;
using OnlineShop.Common.DTOs;
using OnlineShop.DataAccess.EFModels;


namespace OnlineShop.Business.Services
{
    public class ShoppingCartService : IShoppingCartsService
    {
        private readonly OnlineShopContext _context;


        public ShoppingCartService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task AddProductToCart(ShoppingCartItemDto shoppingCartItemDto, int authenticatedUserId)
        {
            if (shoppingCartItemDto.UserId != authenticatedUserId)
            {
                throw new Exception("User is not authorized to perform this operation");
            }
            User user = await _context.Users
                .Include(u => u.ShoppingCart)
                    .ThenInclude(sc => sc.ShoppingCartItems)
                .FirstOrDefaultAsync(u => u.UserId == shoppingCartItemDto.UserId) ?? throw new Exception("User not found");
            Product product = await _context.Products.FindAsync(shoppingCartItemDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            ShoppingCartItem existingItem = user.ShoppingCart.ShoppingCartItems
                .FirstOrDefault(item => item.ProductId == shoppingCartItemDto.ProductId);

            if (existingItem != null)
            {
                int requestedQuantity = existingItem.Quantity + shoppingCartItemDto.Quantity;
                if (requestedQuantity > product.StockQuantity)
                {
                    throw new Exception("Requested quantity exceeds available stock");
                }
                existingItem.Quantity = requestedQuantity;
            }
            else
            {
                if (shoppingCartItemDto.Quantity > product.StockQuantity)
                {
                    throw new Exception("Requested quantity exceeds available stock");
                }

                user.ShoppingCart.ShoppingCartItems.Add(new ShoppingCartItem
                {
                    ProductId = shoppingCartItemDto.ProductId,
                    Quantity = shoppingCartItemDto.Quantity
                });
            }

            await _context.SaveChangesAsync();
        }


        public async Task<ShoppingCartItemDto> UpdateProductQuantity(ShoppingCartItemDto shoppingCartItemDto, int authenticatedUserId)
        {
            if (shoppingCartItemDto.UserId != authenticatedUserId)
            {
                throw new Exception("User is not authorized to perform this operation");
            }

            if (shoppingCartItemDto.UserId != authenticatedUserId)
            {
                throw new Exception("User is not authorized to perform this operation");
            }
            User user = await GetUserWithCart(shoppingCartItemDto.UserId) ?? throw new Exception("User not found");
            Product product = await _context.Products.FindAsync(shoppingCartItemDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            ShoppingCartItem item = user.ShoppingCart.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == shoppingCartItemDto.ProductId) ?? throw new Exception("Shopping cart item not found");

            if (shoppingCartItemDto.Quantity > product.StockQuantity)
            {
                throw new Exception("Requested quantity exceeds available stock");
            }

            item.Quantity = shoppingCartItemDto.Quantity;

            await _context.SaveChangesAsync();

            return ShoppingCartItemToDto(item);
        }
        public async Task RemoveProductFromCart(ShoppingCartItemDto shoppingCartItemDto)
        {
            User user = await GetUserWithCart(shoppingCartItemDto.UserId) ?? throw new Exception("User not found");

            ShoppingCartItem? item = user.ShoppingCart.ShoppingCartItems
                .FirstOrDefault(i => i.ProductId == shoppingCartItemDto.ProductId);

            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
        public async Task ClearCart(int userId)
        {
            User user = await GetUserWithCart(userId) ?? throw new Exception("User not found");

            _context.RemoveRange(user.ShoppingCart.ShoppingCartItems);
            await _context.SaveChangesAsync();
        }
        public async Task<ShoppingCartDto> GetShoppingCart(int userId)
        {
            User user = await GetUserWithCart(userId);
            return user == null
                ? throw new Exception("User not found")
                : new ShoppingCartDto
                {
                    UserId = userId,
                    ShoppingCartItems = user.ShoppingCart.ShoppingCartItems
                    .Select(i => new ShoppingCartItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    })
                    .ToList()
                };
        }

        public async Task<User> GetUserWithCart(int userId)
        {
            return await _context.Users
                .Include(u => u.ShoppingCart)
                .ThenInclude(sc => sc.ShoppingCartItems)
                .FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new Exception("User not found");
        }
        private static ShoppingCartItemDto ShoppingCartItemToDto(ShoppingCartItem shoppingCartItem) =>
            new ShoppingCartItemDto
            {
                ProductId = shoppingCartItem.ProductId,
                Quantity = shoppingCartItem.Quantity,
            };
        
    }
}