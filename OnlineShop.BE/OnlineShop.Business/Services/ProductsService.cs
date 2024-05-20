using Microsoft.EntityFrameworkCore;
using OnlineShop.DataAccess.EFModels;
using OnlineShop.Common.DTOs;

namespace OnlineShop.Business.Services.Interfaces
{
    public class ProductsService : IProductsService
    {
        private readonly OnlineShopContext _context;

        public ProductsService(OnlineShopContext context)
        {
            _context = context;
        }

        private static ProductDto ProductToDto(Product product) =>
            new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl
            };

        private static void UpdateProductFromDto(Product product, ProductDto productDto)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.StockQuantity = productDto.StockQuantity;
            product.ImageUrl = productDto.ImageUrl;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            return await _context.Products
                .Select(p => ProductToDto(p))
                .ToListAsync();
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            return ProductToDto(product);
        }

        public async Task<ProductDto> AddProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                StockQuantity = productDto.StockQuantity,
                ImageUrl = productDto.ImageUrl,
                CreatedDate = DateTime.Now
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return ProductToDto(product);
        }

        public async Task<ProductDto> UpdateProduct(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            UpdateProductFromDto(product, productDto);
            await _context.SaveChangesAsync();

            return ProductToDto(product);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
