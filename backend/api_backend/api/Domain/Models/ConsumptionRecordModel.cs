using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Domain.NewFolder;

public class ConsumptionRecordModel
{
    [Key]
    public string Id { get; set; }

    public DateTime Timestamp { get; set; }

    public double kWValue { get; set; }
}