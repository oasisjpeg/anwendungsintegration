using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Repositories;

public interface IConsumptionRecordRepository
{
    Task<IEnumerable<ConsumptionRecordModel>> GetByIdAsync(string userId);
    Task<ConsumptionRecordModel> AddAsync(ConsumptionRecordModel entity);
}