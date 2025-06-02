using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.API.Controller.Consumption;

[ApiController]
[Route("api/consumption-records")]
public class ConsumptionRecordsController : ControllerBase
{
    private readonly IConsumptionRecordRepository _repository;
    private readonly IUserAuth _userRepository;

    public ConsumptionRecordsController(IConsumptionRecordRepository repository, IUserAuth userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyConsumptionRecords()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Invalid token – user ID not found.");

        var userIdGuid = _userRepository.GetUserIdGuidFromClaims(userId);
        var records = await _repository.GetByIdAsync(userIdGuid);

        if (records == null || !records.Any())
            return NotFound("No consumption records found for this user.");

        Console.Out.WriteLine(records);
        return Ok(records);
    }

    [HttpGet("test")]
    public IActionResult Test() => Ok("It works!");


}
