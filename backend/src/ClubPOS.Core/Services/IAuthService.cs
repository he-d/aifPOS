using ClubPOS.Core.Models;
using System.Threading.Tasks;

namespace ClubPOS.Core.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(User user, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
} 