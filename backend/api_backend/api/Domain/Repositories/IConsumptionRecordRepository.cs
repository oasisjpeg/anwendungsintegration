using WebApplication1.Domain.NewFolder;

namespace WebApplication1.Domain.Repositories;

public interface IConsumptionRecordRepository
{
    Task<IEnumerable<ConsumptionRecordModel>> GetByUserId(int userId);
    Task<ConsumptionRecordModel> AddAsync(ConsumptionRecordModel entity);
}