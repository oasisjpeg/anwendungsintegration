using Microsoft.EntityFrameworkCore;
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
        public async Task<List<ArticleModel>> GetArticleOverviewFromDB()
        {
            return await _dbContext.Articles.ToListAsync();
        }

        public async Task<ArticleModel> GetOneCompleteArticleFromDB(int articleId)
        {
            var article = await _dbContext.Articles.FirstOrDefaultAsync(a => a.ArticleId == articleId);
            if (article == null)
                throw new KeyNotFoundException($"Article with ID {articleId} not found.");
            return article;
        }

        public async Task SubmitOneQuestionAnswerFromDB(QuizQuestionDto quizQuestionDto, Guid userId)
        {
            var question = await _dbContext.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.QuestionId == quizQuestionDto.QuestionId);

            if (question == null)
            {
                throw new Exception("Question not found");
            }

            var userAnswer = new UserAnswerModel
            {
                AnswerId = 0,
                AnsweredAt = DateTime.UtcNow,
                SelectedAnswer = quizQuestionDto.AnswerSelectionIndex,
                QuestionId = quizQuestionDto.QuestionId,
                UserId = int.Parse(userId.ToString()),
                Question = question
            };
            
            _dbContext.UserAnswer.Add(userAnswer);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<bool> IsLastQuestionInQuiz(int quizId, int questionId) 
        {
            var quizQuestions = await _dbContext.Question
                .Where(q => q.QuizId == quizId)
                .OrderBy(q => q.QuestionId)
                .ToListAsync();
            
            return quizQuestions.Count > 0 && 
                   quizQuestions.IndexOf(quizQuestions.FirstOrDefault(q => q.QuestionId == questionId)) == 3; // TODO: handle possible null reference
        }
    }
}
