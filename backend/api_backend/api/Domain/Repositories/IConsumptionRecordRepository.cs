using WebApplication1.Domain.NewFolder;

namespace WebApplication1.Domain.Repositories;

public interface IConsumptionRecordRepository
{
    Task<IEnumerable<ConsumptionRecord>> GetByUserId(int userId);
    Task<ConsumptionRecord> AddAsync(ConsumptionRecord entity);
}