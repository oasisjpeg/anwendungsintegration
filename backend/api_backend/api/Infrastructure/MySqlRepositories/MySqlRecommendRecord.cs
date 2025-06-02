using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.Consumption;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories
{
    public class MySqlRecommendRecord : IRecommendRecordRepository
    {
        private readonly MySqlDbContext _dbContext;

        public MySqlRecommendRecord(MySqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<RecommendRecordModel>> GetRecommendConsumption(string UserId)
        {

            var record = await _dbContext.RecommendRecords
                .Where(r => r.UserId == UserId)
                .OrderByDescending(r => r.Created)
                .ToListAsync();

            return record;
        }
    }
}
