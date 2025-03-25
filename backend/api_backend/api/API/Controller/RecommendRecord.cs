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
        private readonly IConsumptionRecordRepository _repository;

        public RecommendRecordController(IRecommendRecordRepository recommendRepository, IConsumptionRecordRepository repository)
        {
            _recommendRepository = recommendRepository;
            _repository = repository;
        }

        public async Task<ActionResult<ConsumptionRecord>> GetRecommendConsumption(int userId)
        {
            //ReccomendRecord
            var recordsRecommend = await _recommendRepository.GetByUserId(userId);
            //ConsumptionRecord
            var recordsConsumption = await _repository.GetByUserId(userId);

            if (recordsRecommend == null || recordsConsumption == null)
            {
                return BadRequest(new { message = "Could not retrieve data" });
            }

            var recommedation = new
            {
                CurrentConsumption = recordsConsumption,
                PotetialConsumption = recordsRecommend
            };

            return Ok(recommedation);
        }
    }
}
