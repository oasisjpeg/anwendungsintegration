using WebApplication1.Domain.Models.Consumption;

namespace WebApplication1.Application_Layer.Services.ConsumptionData;

public interface IConsumptionDataService
{
    Task<List<ConsumptionRecordModel>> GenerateInitialConsumptionDataAsync(Guid userId);
    Task<List<RecommendRecordModel>> GenerateInitialRecommendationDataAsync(Guid userId);
}