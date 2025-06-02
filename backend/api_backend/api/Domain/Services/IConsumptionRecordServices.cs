using WebApplication1.Domain.Models.Consumption;

namespace WebApplication1.Domain.Repositories;

public interface IConsumptionRecordServices
{
    Task<IEnumerable<ConsumptionRecordModel>> GetByIdAsync(string userId);
    Task<ConsumptionRecordModel> AddAsync(ConsumptionRecordModel entity);
}