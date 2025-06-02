using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.Consumption;
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
    public async Task<IEnumerable<ConsumptionRecordModel>> GetByIdAsync(Guid userId)
    {
        return await _dbContext.ConsumptionRecords
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<ConsumptionRecordModel> AddAsync(ConsumptionRecordModel entity)
    {
        _dbContext.ConsumptionRecords.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
}