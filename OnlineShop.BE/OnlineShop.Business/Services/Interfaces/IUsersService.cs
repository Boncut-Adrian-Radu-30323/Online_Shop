using OnlineShop.Common.DTOs;
using OnlineShop.DataAccess.EFModels;

namespace OnlineShop.Business.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> ValidateUserAsync(string username, string password);
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto> GetById(int id);
        Task<UserDto> Add(UserDto userDto);
        Task<UserDto> Update(int id, UserDto userDto);
        Task Delete(int id);
    }
}
