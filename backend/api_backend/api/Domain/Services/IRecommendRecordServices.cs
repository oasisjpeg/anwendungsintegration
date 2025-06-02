using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Models.Consumption;

namespace WebApplication1.Domain.Repositories
{
    public interface IRecommendRecordServices
    {
        Task<ActionResult<RecommendRecordModel>> GetRecommendConsumption(string userId);
        
    }
}
