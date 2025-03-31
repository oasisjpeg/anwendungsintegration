using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Domain.Models;


public class ConsumptionRecordModel 
{
    [Key]
    public required string ConsumptionId { get; set; }

    [ForeignKey("User")]
    public required string UserId { get; set; }

    public required DateTime Timestamp { get; set; }

    public required double kWValue { get; set; }

    public virtual UserModel Users { get; set; }
}