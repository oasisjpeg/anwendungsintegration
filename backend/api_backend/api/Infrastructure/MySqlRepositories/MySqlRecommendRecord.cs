using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<IEnumerable<RecommendRecordModel>> GetRecommendConsumption(string UserId)
        {

            var record = await _context.RecommendRecords
                .Where(r => r.UserId == UserId)
                .OrderByDescending(r => r.Timestamp)
                .ToListAsync();

            return record;
        }
    }
}
