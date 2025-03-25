using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.NewFolder;

namespace WebApplication1.Domain.Repositories
{
    public interface IRecommendRecordRepository
    {
        Task<ActionResult<ConsumptionRecord>> GetRecommendConsumption(int userId);
        Task<IEnumerable<ConsumptionRecord>> GetByUserId(int userId);
    }
}
