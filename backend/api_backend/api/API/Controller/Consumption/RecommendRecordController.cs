using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.API.Controller.Consumption
{
    [ApiController]
    [Route("api/recommend-records")]
    public class RecommendRecordController : ControllerBase
    {
        private readonly IRecommendRecordRepository _recommendRepository;
        private readonly IUserAuth _userAuth;

        public RecommendRecordController(IRecommendRecordRepository recommendRepository, IUserAuth userAuth)
        {
            _recommendRepository = recommendRepository;
            _userAuth = userAuth;
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
            
            var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userId);
            var recommendConsumption = await _recommendRepository.GetRecommendConsumption(userIdGuid);

            if (recommendConsumption == null)
                return NotFound("No recommendations found.");

            return Ok(recommendConsumption);
        }
    }
}