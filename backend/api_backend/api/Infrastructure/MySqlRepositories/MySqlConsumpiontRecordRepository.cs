using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.NewFolder;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlConsumptionRecordRepository : IConsumptionRecordRepository
{
    private readonly MySqlDbContext _dbContext;

    public MySqlConsumptionRecordRepository(MySqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    // ✅ Add new consumption record
    public async Task<IEnumerable<ConsumptionRecord>> GetByUserId(int userId)
    {
        return await _dbContext.ConsumptionRecords
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<ConsumptionRecord> AddAsync(ConsumptionRecord entity)
    {
        _dbContext.ConsumptionRecords.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
}