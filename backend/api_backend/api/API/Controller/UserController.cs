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
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userModel)
        {
            if (string.IsNullOrWhiteSpace(userModel.CurrentPasswordHash))
                return BadRequest("Password is required.");

            // Hash password before saving
            userModel.CurrentPasswordHash = UserRegisterDto.HashPassword(userModel.CurrentPasswordHash);
            var createdUser = await _userRepository.RegisterAsync(userModel);
            return CreatedAtAction(nameof(Login), new { email = createdUser.Email }, createdUser);
        }

        // ✅ Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRegisterDto userModel)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userModel.Email); // <-- why are we getting user by email if Id is [key]?
            if (existingUser == null || !existingUser.VerifyPassword(userModel.CurrentPasswordHash))
            {
                return Unauthorized("Invalid email or password."); // <-- sepperate into two if statements to facilitate accurate HTTP responses (not found / unauthorized)
        }

            return Ok(new { message = "Login successful!", user = existingUser });
        }

    // DELETE user 
        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> Delete([FromBody] UserAuthDto userAuthDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(userAuthDto.Id);
            // check if user exists
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }
            // authenticate user
            if (!existingUser.VerifyPassword(userAuthDto.CurrentPasswordHash))
            {
                return Unauthorized("Invalid password.");
            }
        // Add delete confirmation ?

        // Delete user
        await _userRepository.DeleteAsync((UserAuthDto)existingUser);

        return Ok(new { message = "Your account has been deleted.", user = existingUser });
        }

    // ADD user UPDATE/PATCH Request

        [HttpPatch("update/{Id}")]
        public async Task<IActionResult> Update([FromBody] UserPatchDto userPatchDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(userPatchDto.Id);
            // check if user exists
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }
            // authenticate user
            if (!existingUser.VerifyPassword(userPatchDto.CurrentPasswordHash))
            {
                return Unauthorized("Invalid password.");
            }

            // Modify user
            await _userRepository.PatchAsync(userPatchDto);

            return Ok(new { message = "Your account has been updated.", user = existingUser });
        }

    // ADD INFORMATION Request
}
