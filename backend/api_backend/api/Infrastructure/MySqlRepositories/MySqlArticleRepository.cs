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

        // Fetches a complete article with its associated quiz and questions
        public async Task<CompleteArticleDto> GetOneCompleteArticleFromDB(int articleId)
        {
            if (articleId <= 0)
                throw new ArgumentException("Article ID must be greater than zero.", nameof(articleId));

            // Get article
            var article = await _dbContext.Articles
                .FirstOrDefaultAsync(a => a.id == articleId);

            if (article == null)
                throw new KeyNotFoundException($"Article with ID {articleId} not found.");

            // Get quiz with questions using Include to ensure proper loading
            var quiz = await _dbContext.Quiz
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.ArticleId == articleId);

            if (quiz == null)
                throw new KeyNotFoundException($"No quiz found for article with ID {articleId}.");

            // Map questions
            var questions = quiz.Questions
                .OrderBy(q => q.id)
                .Select(q => new QuestionModel
                {
                    id = q.id,
                    QuestionText = q.QuestionText,
                    FirstAnswerOption = q.FirstAnswerOption,
                    SecondAnswerOption = q.SecondAnswerOption,
                    ThirdAnswerOption = q.ThirdAnswerOption,
                    FourthAnswerOption = q.FourthAnswerOption,
                    CorrectAnswerIndex = q.CorrectAnswerIndex,
                    QuizId = q.QuizId
                })
                .ToList();

            // Create the quiz model with questions
            var quizModel = new QuizModel
            {
                id = quiz.id,
                Title = quiz.Title,
                ArticleId = quiz.ArticleId,
                Questions = questions
            };

            // Create the article model
            var articleModel = new ArticleModel
            {
                id = article.id,
                Title = article.Title,
                Content = article.Content,
                Url = article.Url,
                DateTime = article.DateTime,
                Description = article.Description
            };

            return new CompleteArticleDto
            {
                Article = articleModel,
                Quiz = quizModel
            };
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
            // Example: Get the quiz ID for the question
            var quizId = await _dbContext.Question
                .Where(q => q.id == questionId)
                .Select(q => q.QuizId)
                .FirstOrDefaultAsync();

            // Find the highest question ID for this quiz
            var maxQuestionId = await _dbContext.Question
                .Where(q => q.QuizId == quizId)
                .MaxAsync(q => (int?)q.id);
            // Return true if this is the last question
            return questionId == maxQuestionId;
        }

    }


}
