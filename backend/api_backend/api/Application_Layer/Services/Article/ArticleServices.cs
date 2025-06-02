using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Transactions;
using WebApplication1.Application_Layer.DTO.Article;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;
using WebApplication1.Infrastructure.MySqlRepositories;

namespace WebApplication1.Application_Layer.Services.Article
{
    public class ArticleServices : IArticleServices
    {
        private IUserRepository _userRepository;
        private MySqlDbContext _dbContext; // TODO: should not be needed --> reference domain layer (MySqlArticleRepository)
        private MySqlArticleRepository _articleRepository;

        public ArticleServices(IUserRepository userRepository, MySqlDbContext dbContext, MySqlArticleRepository articleRepository)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
            _articleRepository = articleRepository;
        }

        public async Task<int> CreateTransaction(Enum sourceType, int sourceId, Guid userId) // TODO: move to TransactionServices ? or at least part of it...
        {
            var userModel = await _userRepository.GetByIdAsync(userId);

            if (userModel == null)
            {
                return 0;
            }

            var newTransaction = new RewardTransactionModel // TODO: move to domain layer?
            {
                TransactionId = 0,
                Created = DateTime.Now,
                PointsGained = 0, // <-- TODO: ADD some sort of way to Calculate Points
                PointSourceType = sourceType,
                PointSourceId = sourceId,
                UserId = userId, 
            };
            int pointsGained = newTransaction.PointsGained;
            return pointsGained;
        }

        public async Task<List<ArticleOverviewDto>> GetArticlesOverview()
        {
            // get overview of articles --> only return list of all articles with Title, Description and URL (for imgs), not entire article content
            return await _dbContext.Articles
            .Select(a => new ArticleOverviewDto
            {
                Title = a.Title,
                Description = a.Description,
                Url = a.Url
            })
            .ToListAsync();
        }

        public async Task<ArticleModel> GetOneCompleteArticleById(int articleId)
        {
            // get one complete article by ID, return entire article object
            var fullArticle = await _articleRepository.GetOneCompleteArticleFromDB(articleId);
            return fullArticle;
        }

        public async Task<bool> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto, Guid userId)
        {
            // submit one question answer to database, for a specific user
            var lastQuestionCheck =
                await _articleRepository.IsLastQuestionInQuiz(quizQuestionDto.QuestionId);
            if (!lastQuestionCheck)
            {
                await _articleRepository.SubmitOneQuestionAnswerToDB(quizQuestionDto, userId);
                return false;
            }
            await _articleRepository.SubmitOneQuestionAnswerToDB(quizQuestionDto, userId);
            return true;
        }
    }
}
