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
        private readonly IArticleRepository _articleRepository;

        public ArticleServices(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<List<ArticleOverviewDto>> GetArticlesOverview()
        {
            // get overview of articles --> only return list of all articles with Title, Description and URL (for imgs), not entire article content
            return await _articleRepository.GetArticleOverviewFromDB();
        }

        public async Task<CompleteArticleDto> GetOneCompleteArticleById(int id)
        {
            // get one complete article by ID, return entire article object
            var fullArticle = await _articleRepository.GetOneCompleteArticleFromDB(id);
            return fullArticle;
        }

        public async Task<bool> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto, Guid userId)
        {
            var isLastQuestion = await _articleRepository.IsLastQuestionInQuiz(quizQuestionDto.QuestionId);
            await _articleRepository.SubmitOneQuestionAnswerToDB(quizQuestionDto, userId);
            return isLastQuestion;
        }

        public Task<bool> IsCorrectAnswer(int questionId, int answerSelection)
        {
            var isCorrect = _articleRepository.IsCorrectAnswer(questionId, answerSelection);
            return isCorrect;
        }
        public async Task<List<QuizAnswersDto>> GetCorrectAnswersForQuiz(int articleId)
        {
            // get correct answers for quiz by article ID and return list of QuizAnswersDto objects
            return await _articleRepository.GetCorrectAnswersForQuiz(articleId);
        }
    }
}