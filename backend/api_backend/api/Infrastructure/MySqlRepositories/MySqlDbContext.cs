using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Models.Consumption;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlDbContext : IdentityDbContext<UserModel, IdentityRole<Guid>, Guid> // --> IdentityDbContext<TUser, TRole, TKey> --> or see IdentityDbContext class for specifics and stuff
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

        // Configure the Quiz-Questions relationship
        builder.Entity<QuizModel>()
            .HasMany(q => q.Questions)
            .WithOne(q => q.Quiz)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the Article-Quiz relationship
        builder.Entity<QuizModel>()
            .HasOne(q => q.Article)
            .WithMany()
            .HasForeignKey(q => q.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        //builder.Entity<RewardTransactionModel>()
        //    .HasOne(r => r.User)
        //    .WithMany()
        //    .HasForeignKey(r => r.UserId);
        // NOTE: above section not usable anymore, bc nav prop was removed --> see RewardTransactionModel        
        // Ef should still be able to handle the FK relationship though

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