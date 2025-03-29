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

    public async Task<IEnumerable<ConsumptionRecordModel>> GetByUserId(int userId)
    {
        return await _context.Set<ConsumptionRecordModel>()
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<ConsumptionRecordModel> AddAsync(ConsumptionRecordModel entity)
    {
        _context.Set<ConsumptionRecordModel>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}