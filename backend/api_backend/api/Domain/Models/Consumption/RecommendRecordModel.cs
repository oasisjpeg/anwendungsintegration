using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Models.Consumption
{
    public class RecommendRecordModel
    {
        // useless? --> [Key]
        public required string id { get; set; }

        // useless? --> [ForeignKey("Users")]
        // fk
        public required Guid UserId { get; set; }

        public required DateTime Created { get; set; }

        public required double KWValue { get; set; }

        // navigation prop
        public virtual UserModel Users { get; set; }
    }
}
