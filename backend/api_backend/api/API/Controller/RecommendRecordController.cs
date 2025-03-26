using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.NewFolder;
using WebApplication1.Domain.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WebApplication1.API.Controller
{
    [ApiController]
    [Route("api/consumption-records")] 
    public class RecommendRecordController : ControllerBase
    {

        private readonly IRecommendRecordRepository _recommendRepository;

        public RecommendRecordController(IRecommendRecordRepository recommendRepository)
        {
            _recommendRepository = recommendRepository;
        }

        //get uuid from jwt token body, so valmir doesnt cry :)
        [HttpGet]
        public async Task<ActionResult<ConsumptionRecordModel>> GetRecommendConsumption(int userId)
        {
            //userId = jwt.token.body
            var recommendConsumpion = _recommendRepository.GetRecommendConsumption(userId);

            if (recommendConsumpion == null) 
                return NotFound();

            return Ok(recommendConsumpion);
        }
    }
}
