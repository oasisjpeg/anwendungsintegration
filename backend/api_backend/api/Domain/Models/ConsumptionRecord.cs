using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Domain.NewFolder;

public class ConsumptionRecord
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public double kWValue { get; set; }
}