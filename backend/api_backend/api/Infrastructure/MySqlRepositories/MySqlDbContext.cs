using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models;

namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlDbContext: IdentityDbContext<UserModel>
{
    public DbSet<ConsumptionRecordModel> ConsumptionRecords { get; set; }
    public DbSet<RecommendRecordModel> RecommendRecords { get; set; }
    public new DbSet<UserModel> Users { get; set; }

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Explicitly defining the foreign key relationship for RecommendRecordModel
        builder.Entity<RecommendRecordModel>()
            .HasOne(r => r.Users)  
            .WithMany()
            .HasForeignKey(r => r.UserId);

        // Explicitly defining the foreign key relationship for ConsumptionRecordModel
        builder.Entity<ConsumptionRecordModel>()
            .HasOne(r => r.Users)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}