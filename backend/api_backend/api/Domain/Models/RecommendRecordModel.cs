using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Domain.Models
{
    public class RecommendRecordModel
    {
        [Key]
        public int UserId { get; set; }

        public DateTime Timestamp { get; set; }

        public double kWValue { get; set; }
    }
}
