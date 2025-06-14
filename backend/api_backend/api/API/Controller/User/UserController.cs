using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Application_Layer.DTO.User;
using WebApplication1.Application_Layer.Services.Leaderboard;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Application_Layer.Services.UserExistCheck;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.API.Controller.User;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserAuth _userAuth;
    private readonly IUserExistCheck _userExistCheck;
    private readonly ILeaderboardServices _leaderboardServices;

    public UserController(IUserRepository userRepository, IUserAuth userAuth, IUserExistCheck userExistCheck, ILeaderboardServices leaderboardServices)
    {
        _userRepository = userRepository;
        _userAuth = userAuth;
        _userExistCheck = userExistCheck;
        _leaderboardServices = leaderboardServices;
    }

    // Register a new user
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
        var hashedPassword = _userAuth.HashPassword(userRegisterDto.Password);
        var createdUser = await _userRepository.RegisterAsync(userRegisterDto.Name, userRegisterDto.Email, hashedPassword);
        return CreatedAtAction(nameof(Login), new { email = createdUser.Email }, createdUser);
    }

    // Login user
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
        if (!_userAuth.VerifyPassword(existingUser.PasswordHash, userAuthDto.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = jwtAuth.GenerateToken(existingUser);
        var refreshToken = jwtAuth.GenerateRefreshToken();
        
        // Store refresh token with 7 days expiry
        await _userRepository.UpdateRefreshTokenAsync(existingUser.Id, refreshToken, DateTime.UtcNow.AddDays(7));

        // TODO: Add Leaderboard call

        return Ok(new { message = "Login successful!", email = existingUser.Email, token, refreshToken });
    }

    // Refresh Token
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, [FromServices] JwtAuth jwtAuth)
    {
        if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
        {
            return BadRequest("Token and refresh token are required.");
        }

        try
        {
            var principal = jwtAuth.GetPrincipalFromExpiredToken(request.Token);
            var userIdString = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid token.");
            }

            var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken);

            if (user == null || user.Id != userId || 
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token.");
            }

            var newAccessToken = jwtAuth.GenerateToken(user);
            var newRefreshToken = jwtAuth.GenerateRefreshToken();

            await _userRepository.UpdateRefreshTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return Ok(new { token = newAccessToken, refreshToken = newRefreshToken });
        }
        catch (Exception)
        {
            return BadRequest("Invalid token.");
        }
    }

    // DELETE user 
    [Authorize]
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
        if (!_userAuth.VerifyPassword(existingUser.PasswordHash, userAuthDto.Password))
        {
            return Unauthorized("Invalid email or password.");
        }
        // Add delete confirmation ?

        // Delete user
        await _userRepository.DeleteAsync(userAuthDto.Email);

        return Ok(new { message = "Your account has been deleted.", user = existingUser });
    }

    // user UPDATE/PATCH Request
    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> Update([FromBody] UserPatchDto userPatchDto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized("Invalid token.");

        var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userIdString);

        var existingUser = await _userRepository.GetByIdAsync(userIdGuid);
        if (existingUser == null)
            return NotFound("User not found.");

        string? hashedNewPassword = null;
        if (!string.IsNullOrWhiteSpace(userPatchDto.NewPassword))
        {
            hashedNewPassword = _userAuth.HashPassword(userPatchDto.NewPassword);
        }
        
        await _userRepository.PatchAsync(userIdGuid, userPatchDto.NewName, userPatchDto.NewEmail, hashedNewPassword);

        return Ok(new { message = "Your account has been updated.", user = existingUser });
    }


    // INFORMATION Request

    [Authorize]
    [HttpGet("information")]
    public async Task<IActionResult> GetInformationOfUser(UserAuthDto userAuthDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value; // Adjust to use primary key instead of email

        if (!await _userExistCheck.UserExistsAsync(userAuthDto.Email))
        {
            return NotFound("User not found.");
        }

        var existingUser = await _userRepository.GetByEmailAsync(userAuthDto.Email);
        // User Auth
        if (string.IsNullOrEmpty(existingUser.PasswordHash))
        {
            return Unauthorized("Invalid user credentials.");
        }
        
        if (!_userAuth.VerifyPassword(existingUser.PasswordHash, userAuthDto.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(existingUser);
    }

    [Authorize]
    [HttpGet("transactions/{quantity}")]
    // !!! quantity is optional !!!
    // NOTE: quantity == amount of transactions to get --> 10 is default
    public async Task<IActionResult> GetRecentTransactionsOfUser(int? quantity)
    {
        // read userId from JWT claims --> use UserId to retrieve
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized("Invalid token.");

        var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userIdString);

        var existingUser = await _userRepository.GetByIdAsync(userIdGuid);
        if (existingUser == null)
            return NotFound("User not found.");

        if (quantity > 100) // prevent getting ridiculous amount of transactions 
        {
            quantity = 100;
        }
        if (quantity == null) // set default to 10 if no number specified
        {
            quantity = 10;
        }

        var transactionList = await _userRepository.GetRecentTransactionsAsync(userIdGuid, (int)quantity);
        
        if (transactionList.Count == 0)
        {
            return NotFound("No transactions found for this user.");
        }

        return Ok(transactionList);
    }
    [Authorize]
    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboardForCurrentUser()
    {
        // read userId from JWT claims --> use UserId to retrieve
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized("Invalid token.");

        var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userIdString);

        var existingUser = await _userRepository.GetByIdAsync(userIdGuid);
        if (existingUser == null)
            return NotFound("User not found.");

        var LeaderboardData = await _leaderboardServices.GetLeaderboardForUser(userIdGuid);

            return Ok(LeaderboardData);
    }
}
