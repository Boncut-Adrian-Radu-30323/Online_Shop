using OnlineShop.Common.DTOs;

namespace OnlineShop.Business.Services.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> AddProduct(ProductDto productDto);
        Task<ProductDto> UpdateProduct(int id, ProductDto productDto);
        Task DeleteProduct(int id);
    }
}
