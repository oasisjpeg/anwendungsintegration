using WebApplication1.Domain.Models.Consumption;

namespace WebApplication1.Application_Layer.Services.ConsumptionData;

public class ConsumptionDataService : IConsumptionDataService
{
    private static readonly DateTime BaseDate = DateTime.Parse("2025-03-13 00:00:00");

    public async Task<List<ConsumptionRecordModel>> GenerateInitialConsumptionDataAsync(Guid userId)
    {
        var (consumptionValues, _) = GenerateDuckCurveData();
        var consumptionRecords = new List<ConsumptionRecordModel>();

        for (int i = 0; i < 24; i++)
        {
            consumptionRecords.Add(new ConsumptionRecordModel
            {
                id = Guid.NewGuid().ToString(),
                UserId = userId,
                Timestamp = BaseDate.AddHours(i),
                KWValue = consumptionValues[i]
            });
        }

        return await Task.FromResult(consumptionRecords);
    }

    public async Task<List<RecommendRecordModel>> GenerateInitialRecommendationDataAsync(Guid userId)
    {
        var (_, recommendedValues) = GenerateDuckCurveData();
        var recommendationRecords = new List<RecommendRecordModel>();

        for (int i = 0; i < 24; i++)
        {
            recommendationRecords.Add(new RecommendRecordModel
            {
                id = Guid.NewGuid().ToString(),
                UserId = userId,
                Created = BaseDate.AddHours(i),
                KWValue = recommendedValues[i]
            });
        }

        return await Task.FromResult(recommendationRecords);
    }

    private (double[] consumption, double[] recommended) GenerateDuckCurveData()
    {
        var hours = Enumerable.Range(0, 24).Select(h => (double)h).ToArray();

        // Baseline load pattern
        var baseline = hours.Select(h => 
                1.2 + 0.8 * Math.Sin((h - 6) / 24 * 2 * Math.PI) // Morning/evening peaks
        ).ToArray();

        // Solar generation dip
        var solarDip = hours.Select(h => 
                1.5 * Math.Exp(-0.5 * Math.Pow((h - 13) / 2.5, 2)) // Peak at 13:00
        ).ToArray();

        // Net consumption (baseline + evening peak - solar generation)
        var netConsumption = hours.Select((h, i) => 
            baseline[i] + 0.5 * Math.Exp(-0.5 * Math.Pow((h - 19)/2, 2)) - solarDip[i]
        ).ToArray();

        // Normalize to match Excel data range (3-13 kWh daily total)
        var minConsumption = netConsumption.Min();
        var maxConsumption = netConsumption.Max();
        var scaledConsumption = netConsumption.Select(c => 
            0.2 + (c - minConsumption) * (1.8 - 0.2) / (maxConsumption - minConsumption)
        ).ToArray();

        // Create recommendation curve (encourage shifting to solar hours)
        var maxRec = scaledConsumption.Max();
        var recommended = scaledConsumption.Select(c => 
            Math.Round(maxRec - c + 0.3, 1)
        ).ToArray();

        return (scaledConsumption, recommended);
    }
}