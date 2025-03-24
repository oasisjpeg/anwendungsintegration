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
            var existingUser = await _userRepository.GetByEmailAsync(userModel.Email); // <-- why are we getting user by email if Id is [key]?
            if (existingUser == null || !existingUser.VerifyPassword(userModel.PasswordHash))
            {
                return Unauthorized("Invalid email or password."); // <-- sepperate into two if statements to facilitate accurate HTTP responses (not found / unauthorized)
        }

            return Ok(new { message = "Login successful!", user = existingUser });
        }

    // DELETE user 
        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var existingUser = await _userRepository.GetByIdAsync(Id);
            // check if user exists
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }
            // authenticate user
            if (!existingUser.VerifyPassword(existingUser.PasswordHash))
            {
                return Unauthorized("Invalid password.");
            }
        // Add delete confirmation ?

        // Delete user
        await _userRepository.DeleteAsync(existingUser);

        return Ok(new { message = "Your account has been deleted.", user = existingUser });
        }

    // ADD user UPDATE/PATCH Request

        [HttpPatch("update/{Id}")]
        public async Task<IActionResult> Update(string Id, [FromBody] IUserUpdateDto updateDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(Id);
            // check if user exists
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }
            // authenticate user
            if (!existingUser.VerifyPassword(updateDto.CurrentPasswordHash))
            {
                return Unauthorized("Invalid password.");
            }

            // Modify user
            await _userRepository.PatchAsync(Id, updateDto);

            return Ok(new { message = "Your account has been updated.", user = existingUser });
        }

    // ADD INFORMATION Request
}
