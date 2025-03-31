using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.API.Controller
{
    [ApiController]
    [Route("api/recommend-records")]
    public class RecommendRecordController : ControllerBase
    {
        private readonly IRecommendRecordRepository _recommendRepository;

        public RecommendRecordController(IRecommendRecordRepository recommendRepository)
        {
            _recommendRepository = recommendRepository;
        }

        // ✅ Require JWT token
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetRecommendConsumption()
        {
            // ✅ Extract user UUID from token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Missing user ID in JWT.");

            var recommendConsumption = await _recommendRepository.GetRecommendConsumption(userId);

            if (recommendConsumption == null)
                return NotFound("No recommendations found.");

            return Ok(recommendConsumption);
        }
    }
}