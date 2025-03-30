using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Application_Layer.DTO;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Application_Layer.Services.UserExistCheck;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.API.Controller;

    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAuth _userAuth;
        private readonly IUserExistCheck _userExistCheck;

    public UserController(IUserRepository userRepository, IUserAuth userAuth, IUserExistCheck userExistCheck)
    {
        _userRepository = userRepository;
        _userAuth = userAuth;
        _userExistCheck = userExistCheck;
    }

    // ✅ Register a new user
    [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
        // Check if user already exists
            if (await _userExistCheck.UserExistsAsync(userRegisterDto.Email))
                return BadRequest("User already exists.");
        // Check if password is empty
            if (string.IsNullOrWhiteSpace(userRegisterDto.Password))
                return BadRequest("Password is required.");

        // Hash password before saving
            userRegisterDto.Password = _userAuth.HashPassword(userRegisterDto.Password);
            var createdUser = await _userRepository.RegisterAsync(userRegisterDto);
            return CreatedAtAction(nameof(Login), new { email = createdUser.Email }, createdUser);
        }

        // ✅ Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthDto userAuthDto, [FromServices] JwtAuth jwtAuth)
        {

            // User Exist Check
            if (!await _userExistCheck.UserExistsAsync(userAuthDto.Email))
            {
                return NotFound("User not found.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(userAuthDto.Email);

            // User Auth
            if (!_userAuth.VerifyPassword(existingUser.CurrentPasswordHash, userAuthDto.Password))
            {
                return Unauthorized("Invalid email or password."); 
            }

            var token = jwtAuth.GenerateToken(existingUser);

            return Ok(new { message = "Login successful!", user = existingUser });
        }

    // DELETE user 
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] UserAuthDto userAuthDto)
        {
            
        // check if user exists
            if (!await _userExistCheck.UserExistsAsync(userAuthDto.Email))
            {
                return NotFound("User not found.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(userAuthDto.Email);
        // User Auth
            if (!_userAuth.VerifyPassword(existingUser.CurrentPasswordHash, userAuthDto.Password)) 
            {
                return Unauthorized("Invalid email or password.");
            }
        // Add delete confirmation ?

        // Delete user
        await _userRepository.DeleteAsync(userAuthDto);

        return Ok(new { message = "Your account has been deleted.", user = existingUser });
        }

    // ADD user UPDATE/PATCH Request

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] UserPatchDto userPatchDto)
        {
            
        // check if user exists
            if (!await _userExistCheck.UserExistsAsync(userPatchDto.Email))
            {
                return NotFound("User not found.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(userPatchDto.Email);
        // User Auth
            if (!_userAuth.VerifyPassword(existingUser.CurrentPasswordHash, userPatchDto.Password)) 
            {
                return Unauthorized("Invalid email or password.");
            }

        // Modify user
        await _userRepository.PatchAsync(userPatchDto);

            return Ok(new { message = "Your account has been updated.", user = existingUser });
        }

    // ADD INFORMATION Request


        [HttpGet("information")]
        public async Task<IActionResult> GetInformationFromUser(UserAuthDto userAuthDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (!await _userExistCheck.UserExistsAsync(userAuthDto.Email))
            {
                return NotFound("User not found.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(userAuthDto.Email);
            // User Auth
            if (!_userAuth.VerifyPassword(existingUser.CurrentPasswordHash, userAuthDto.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(existingUser);
        }

    }
