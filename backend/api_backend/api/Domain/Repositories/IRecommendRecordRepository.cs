using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Repositories
{
    public interface IRecommendRecordRepository
    {
        Task<ActionResult<RecommendRecordModel>> GetRecommendConsumption(string userId);
        
    }
}
