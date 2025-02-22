using Microsoft.AspNetCore.Mvc;
using PokesMan.Dto;
using PokesMan.Models;
using PokesMan.Repository;
using PokesMan.Services;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace PokesMan.Controllers
{
    [ApiController] 
    [Route("api/auth")] 
    public class AuthController : ControllerBase 
    {
        private readonly AuthRepository _authRepo;
        private readonly TokenService _tokenService;

        public AuthController(AuthRepository authRepo, TokenService tokenService)
        {
            _authRepo = authRepo;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var existingUser = await _authRepo.GetUserByEmail(model.Email);
            if (existingUser != null) return BadRequest("User already exists");

            var hashedPassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(model.Password)));
            var user = new User { Email = model.Email, PasswordHash = hashedPassword };

            await _authRepo.Register(user);
            return Ok("User Registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _authRepo.GetUserByEmail(model.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            var hashedPassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(model.Password)));
            if (user.PasswordHash != hashedPassword) return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);

            var userResponse = new
            {
                user.Id,
                user.Email
            };
            return Ok(new { Token = token, User=userResponse });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginModel model)
        {
            var user = await _authRepo.GetUserByEmail(model.Email);
            if (user == null) return NotFound("User Not found");

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);
            await _authRepo.UpdateUser(user);

            return Ok($"Reset token: {user.ResetPasswordToken}");
        }

        [HttpPost("reset-password")] 
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _authRepo.GetUserByEmail(model.Email);
            if (user == null || user.ResetPasswordToken != model.Token || user.ResetPasswordExpiry < DateTime.UtcNow)
                return BadRequest("Invalid or expired token");

            user.PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(model.NewPassword)));
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiry = null;
            await _authRepo.UpdateUser(user);

            return Ok("Password reset successfully");
        }
    }
}
