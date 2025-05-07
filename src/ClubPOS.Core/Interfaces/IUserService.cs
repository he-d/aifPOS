using System.Threading.Tasks;
using ClubPOS.Core.Models;

namespace ClubPOS.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(User user, string password);
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task UpdatePasswordAsync(int userId, string currentPassword, string newPassword);
        Task DeleteAsync(int id);
        Task<bool> ValidatePasswordAsync(string password, string hash, string salt);
        Task<string> GenerateJwtTokenAsync(User user);
    }
} 