using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.API.Controller;

    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel userModel)
        {
            if (string.IsNullOrWhiteSpace(userModel.PasswordHash))
                return BadRequest("Password is required.");

            // Hash password before saving
            userModel.PasswordHash = UserModel.HashPassword(userModel.PasswordHash);
            var createdUser = await _userRepository.RegisterAsync(userModel);
            return CreatedAtAction(nameof(Login), new { email = createdUser.Email }, createdUser);
        }

        // ✅ Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel userModel)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userModel.Email);
            if (existingUser == null || !existingUser.VerifyPassword(userModel.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { message = "Login successful!", user = existingUser });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetInformationFromUser(int userId)
        {
            var user = await _userRepository.GetByUserId(userId);

            if (userId == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

    }
