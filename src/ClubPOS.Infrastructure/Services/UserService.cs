using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClubPOS.Core.Interfaces;
using ClubPOS.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClubPOS.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return null;

            if (!await ValidatePasswordAsync(password, user.PasswordHash, user.Salt))
                return null;

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return user;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            if (await _userRepository.ExistsByUsernameAsync(user.Username))
                throw new Exception("Username already exists");

            if (await _userRepository.ExistsByEmailAsync(user.Email))
                throw new Exception("Email already exists");

            CreatePasswordHash(password, out string passwordHash, out string salt);

            user.PasswordHash = passwordHash;
            user.Salt = salt;
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            return await _userRepository.AddAsync(user);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new Exception("User not found");

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.IsActive = user.IsActive;

            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task UpdatePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            if (!await ValidatePasswordAsync(currentPassword, user.PasswordHash, user.Salt))
                throw new Exception("Current password is incorrect");

            CreatePasswordHash(newPassword, out string passwordHash, out string salt);

            user.PasswordHash = passwordHash;
            user.Salt = salt;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _userRepository.ExistsAsync(id))
                throw new Exception("User not found");

            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidatePasswordAsync(string password, string hash, string salt)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(salt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == hash;
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void CreatePasswordHash(string password, out string passwordHash, out string salt)
        {
            using var hmac = new HMACSHA512();
            salt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
} 