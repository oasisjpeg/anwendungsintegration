using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.NewFolder;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories
{
    public class MySqlRecommendRecord : IRecommendRecordRepository
    {
        private readonly MySqlDbContext _context;

        public MySqlRecommendRecord(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<ConsumptionRecordModel>> GetRecommendConsumption(int userId)
        {
            return await _context.ConsumptionRecords
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Id)
                .FirstOrDefaultAsync();
        }

    }
}
