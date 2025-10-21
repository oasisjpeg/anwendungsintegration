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
    private readonly IUserAuth _userAuth;

    public ConsumptionRecordsController(IConsumptionRecordRepository repository, IUserAuth userAuth)
    {
        _repository = repository;
        _userAuth = userAuth;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyConsumptionRecords()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Invalid token – user ID not found.");

        var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userId);
        var records = await _repository.GetByIdAsync(userIdGuid);

        if (records == null || !records.Any())
            return NotFound("No consumption records found for this user.");

        Console.Out.WriteLine(records);
        return Ok(records);
    }
    
}
