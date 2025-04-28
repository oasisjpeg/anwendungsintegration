using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Models.Consumption;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlDbContext: IdentityDbContext<UserModel>
{
    public DbSet<ConsumptionRecordModel> ConsumptionRecords { get; set; }
    public DbSet<RecommendRecordModel> RecommendRecords { get; set; }
    // public new DbSet<UserModel> Users { get; set; } --> not needed cause DbContext already inherits from IdentityDbContext<UserModel> 
    public DbSet<ArticleModel> Articles { get; set; }
    public DbSet<QuestionModel> Question { get; set; }
    public DbSet<QuizModel> Quiz { get; set; }
    public DbSet<RewardTransactionModel> RewardTransactions { get; set; }
    public DbSet<UserAnswerModel> UserAnswer { get; set; }

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Explicitly defining the foreign key relationship for ConsumptionRecordModel
        builder.Entity<ConsumptionRecordModel>()
            .HasOne(r => r.Users)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        // Explicitly defining the foreign key relationship for RecommendRecordModel
        builder.Entity<RecommendRecordModel>()
            .HasOne(r => r.Users)  
            .WithMany()
            .HasForeignKey(r => r.UserId);

        builder.Entity<QuestionModel>()
            .HasOne(r => r.Quiz)
            .WithMany()
            .HasForeignKey(r => r.QuizId);

        builder.Entity<QuizModel>()
            .HasOne(r => r.Article)
            .WithMany()
            .HasForeignKey(r => r.ArticleId);

        builder.Entity<RewardTransactionModel>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        builder.Entity<UserAnswerModel>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
        builder.Entity<UserAnswerModel>()
            .HasOne(r => r.Question)
            .WithMany()
            .HasForeignKey(r => r.QuestionId);


    }
}