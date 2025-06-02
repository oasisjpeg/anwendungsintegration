using WebApplication1.Domain.Models.Consumption;

namespace WebApplication1.Domain.Repositories;

public interface IConsumptionRecordRepository
{
    Task<IEnumerable<ConsumptionRecordModel>> GetByIdAsync(Guid userId);
    Task<ConsumptionRecordModel> AddAsync(ConsumptionRecordModel entity);
}