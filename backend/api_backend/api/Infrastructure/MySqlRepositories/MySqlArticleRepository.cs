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
            var newTransaction = new RewardTransactionModel
            {
                id = 0,
                Created = DateTime.Now,
                PointsGained = 0, // Points will be calculated in service layer
                PointSourceType = pointSourceType,
                PointSourceId = sourceId,
                UserId = userId
            };
            
            _dbContext.RewardTransactions.Add(newTransaction);
            await _dbContext.SaveChangesAsync();
            return newTransaction.PointsGained;
        }

        public async Task<List<ArticleOverviewDto>> GetArticleOverviewFromDB()
        {
            return await _dbContext.Articles
            .Select(a => new ArticleOverviewDto
            {
                id = a.id,
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

        public async Task SubmitOneQuestionAnswerToDB(QuizQuestionDto quizQuestionDto, Guid userId)
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
        
        public async Task<bool> IsLastQuestionInQuiz(int questionId) 
        {
            // Flexible Lösung für variable Anzahl von Fragen pro Quiz (aktuell 4)
            var question = await _dbContext.Question.FirstOrDefaultAsync(q => q.id == questionId);
            if (question == null)
                throw new KeyNotFoundException($"Question with ID {questionId} not found.");

            var totalQuestionsInQuiz = await _dbContext.Question
                .CountAsync(q => q.QuizId == question.QuizId);
            
            var currentQuestionIndex = await _dbContext.Question
                .Where(q => q.QuizId == question.QuizId)
                .OrderBy(q => q.id)
                .Select(q => q.id)
                .ToListAsync();

            return currentQuestionIndex.IndexOf(questionId) == totalQuestionsInQuiz - 1;
        }
    }
}
