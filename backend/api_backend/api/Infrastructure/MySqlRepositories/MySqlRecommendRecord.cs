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

        public async Task<ActionResult<ConsumptionRecordModel>> GetRecommendConsumption(string Id)
        {
            var recordExists = await _context.ConsumptionRecords.AnyAsync(r => r.Id == Id);
            if (!recordExists)
            {
                return new NotFoundResult();
            }

            var record = await _context.ConsumptionRecords
                .Where(r => r.Id == Id)
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefaultAsync();

            return record != null ? new OkObjectResult(record) : new NotFoundResult();
        }
    }
}
