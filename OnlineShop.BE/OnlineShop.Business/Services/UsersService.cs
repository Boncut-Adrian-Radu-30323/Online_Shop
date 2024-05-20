using System.Security.Cryptography;
using System.Text;
using OnlineShop.DataAccess.EFModels;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Common.DTOs;

namespace OnlineShop.Business.Services.Interfaces
{
    public class UsersService : IUsersService
    {
        private readonly OnlineShopContext _context;

        public UsersService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<UserDto> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return UserToDto(user);
            }
            return null;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hashedPassword == storedHash;
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static UserDto UserToDto(User user) =>
            new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                IsAdmin = user.IsAdmin,
                CreatedDate = user.CreatedDate
            };

        private static void UpdateUserFromDto(User user, UserDto userDto)
        {
            user.Username = userDto.Username;
            user.PasswordHash = HashPassword(userDto.Password);
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Address = userDto.Address;
            user.PhoneNumber = userDto.PhoneNumber;
            user.IsAdmin = userDto.IsAdmin;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return await _context.Users
                .Select(x => UserToDto(x))
                .ToListAsync();
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return UserToDto(user);
        }

        public async Task<UserDto> Update(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            UpdateUserFromDto(user, userDto);
            await _context.SaveChangesAsync();

            return UserToDto(user);
        }

        public async Task<UserDto> Add(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = HashPassword(userDto.Password),
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Address = userDto.Address,
                PhoneNumber = userDto.PhoneNumber,
                IsAdmin = userDto.IsAdmin,
                CreatedDate = userDto.CreatedDate,
                ShoppingCart= new ShoppingCart()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return UserToDto(user);
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
