using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.NewFolder;

namespace WebApplication1.Domain.Repositories
{
    public interface IRecommendRecordRepository
    {
        Task<ActionResult<ConsumptionRecordModel>> GetRecommendConsumption(int userId);
        
    }
}
