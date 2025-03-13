using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.NewFolder;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MockRepositories;

public class ConsumptionRecordRepository : IConsumptionRecordRepository
{
    private readonly DbContext _context;

    public ConsumptionRecordRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ConsumptionRecord>> GetByUserId(int userId)
    {
        return await _context.Set<ConsumptionRecord>()
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<ConsumptionRecord> AddAsync(ConsumptionRecord entity)
    {
        _context.Set<ConsumptionRecord>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}