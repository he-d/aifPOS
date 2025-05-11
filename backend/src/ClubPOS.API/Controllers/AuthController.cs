using ClubPOS.Core.Models;
using ClubPOS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] User user, [FromQuery] string password)
        {
            var result = await _authService.RegisterAsync(user, password);
            if (!result)
            {
                return BadRequest("Username already exists");
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromQuery] string currentPassword, [FromQuery] string newPassword)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _authService.ChangePasswordAsync(userId, currentPassword, newPassword);
            if (!result)
            {
                return BadRequest("Current password is incorrect");
            }
            return Ok();
        }
    }
} 