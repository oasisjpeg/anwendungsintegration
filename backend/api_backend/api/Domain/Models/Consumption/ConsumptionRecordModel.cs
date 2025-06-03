using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Models.Consumption;


public class ConsumptionRecordModel
{
    [Key]
    public required string id { get; set; }

    [ForeignKey("Users")]
    public required Guid UserId { get; set; }

    public required DateTime Timestamp { get; set; }

    public required double KWValue { get; set; }

    public virtual UserModel Users { get; set; }
}