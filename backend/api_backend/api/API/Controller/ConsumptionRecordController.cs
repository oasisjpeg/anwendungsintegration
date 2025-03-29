using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.API.Controller;

[ApiController]
[Route("api/consumption-records")]
public class ConsumptionRecordsController : ControllerBase
{
    private readonly IConsumptionRecordRepository _repository;

    public ConsumptionRecordsController(IConsumptionRecordRepository repository)
    {
        _repository = repository;
    }

    // ✅ Get records by user ID
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUser(string Id)
    {
        var records = await _repository.GetByIdAsync(Id);
        if (records == null || !records.Any()) return NotFound();
        return Ok(records);
    }
}
