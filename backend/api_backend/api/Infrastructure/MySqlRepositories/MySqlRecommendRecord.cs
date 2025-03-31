using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models;
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

        public async Task<ActionResult<RecommendRecordModel>> GetRecommendConsumption(string UserId)
        {
            var recordExists = await _context.RecommendRecords.AnyAsync(r => r.UserId == UserId);
            if (!recordExists)
            {
                return new NotFoundResult();
            }

            var record = await _context.RecommendRecords
                .Where(r => r.UserId == UserId)
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefaultAsync();

            return record != null ? new OkObjectResult(record) : new NotFoundResult();
        }
    }
}
