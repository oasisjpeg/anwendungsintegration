using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using WebApplication1.Application_Layer.DTO.Article;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories
{
    public class MySqlArticleRepository : IArticleRepository
    {
        private readonly MySqlDbContext _dbContext;
        public MySqlArticleRepository(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateTransaction(PointSourceType pointSourceType, int sourceId, Guid userId)
        {
            var newTransaction = new RewardTransactionModel // TODO: move to domain layer?
            {
                id = 0,
                Created = DateTime.Now,
                PointsGained = 0, // <-- TODO: ADD some sort of way to Calculate Points --> add method
                PointSourceType = pointSourceType,
                PointSourceId = sourceId,
                UserId = userId
            };
            int pointsGained = newTransaction.PointsGained;
            await _dbContext.SaveChangesAsync();
            return pointsGained;
        }

        public async Task<List<ArticleOverviewDto>> GetArticleOverviewFromDB()
        {
            return await _dbContext.Articles
            .Select(a => new ArticleOverviewDto
            {
                Title = a.Title,
                Description = a.Description,
                Url = a.Url
            })
            .ToListAsync();
        }

        public async Task<ArticleModel> GetOneCompleteArticleFromDB(int articleId)
        {
            var article = await _dbContext.Articles.FirstOrDefaultAsync(a => a.id == articleId);
            if (article == null)
                throw new KeyNotFoundException($"Article with ID {articleId} not found.");
            return article;
        }

        public async Task SubmitOneQuestionAnswerFromDB(QuizQuestionDto quizQuestionDto, Guid userId)
        {
            var question = await _dbContext.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.id == quizQuestionDto.QuestionId);

            if (question == null)
            {
                throw new Exception("Question not found");
            }

            var userAnswer = new UserAnswerModel
            {
                id = 0,
                AnsweredAt = DateTime.UtcNow,
                SelectedAnswer = quizQuestionDto.AnswerSelectionIndex,
                QuestionId = quizQuestionDto.QuestionId,
                UserId = userId,
                Question = question
            };
            
            _dbContext.UserAnswer.Add(userAnswer);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<bool> IsLastQuestionInQuiz(int quizId, int questionId) 
        {
            var quizQuestions = await _dbContext.Question
                .Where(q => q.QuizId == quizId)
                .OrderBy(q => q.id)
                .ToListAsync();
            
            return quizQuestions.Count > 0 && 
                   quizQuestions.IndexOf(quizQuestions.FirstOrDefault(q => q.id == questionId)) == 3; // TODO: handle possible null reference
        }
    }
}
