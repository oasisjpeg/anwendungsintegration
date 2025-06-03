using Microsoft.AspNetCore.Http.HttpResults;
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
        private IArticleRepository _articleRepository;

        public ArticleServices(IUserRepository userRepository, IArticleRepository articleRepository)
        {
            _userRepository = userRepository;
            _articleRepository = articleRepository;
        }

        public async Task<int> CreateTransaction(PointSourceType sourceType, int sourceId, Guid userId) // TODO: move to TransactionServices ? or at least part of it...
        {
            var userModel = await _userRepository.GetByIdAsync(userId);

            if (userModel == null)
            {
                throw new Exception("User not found.");
            }

            var pointsGained = await _articleRepository.CreateTransaction(sourceType, sourceId, userId);
            return pointsGained;
        }

        public async Task<List<ArticleOverviewDto>> GetArticlesOverview()
        {
            // get overview of articles --> only return list of all articles with Title, Description and URL (for imgs), not entire article content
            return await _articleRepository.GetArticleOverviewFromDB();
        }

        public async Task<ArticleWithQuizModel> GetOneCompleteArticleById(int articleId)
        {
            // get one complete article by ID, return entire article object
            var fullArticle = await _articleRepository.GetOneCompleteArticleFromDB(articleId);
            return fullArticle;
        }

        public async Task<bool> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto, Guid userId)
        {
            var isLastQuestion = await _articleRepository.IsLastQuestionInQuiz(quizQuestionDto.QuestionId);
            await _articleRepository.SubmitOneQuestionAnswerToDB(quizQuestionDto, userId);
            return isLastQuestion;
        }

    }
}
