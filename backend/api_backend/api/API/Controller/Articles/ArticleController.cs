using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Application_Layer.Services.Article;
using WebApplication1.Application_Layer.Services.Transaction;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.API.Controller.Articles
{
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private IArticleServices _articleServices;
        private IUserAuth _userAuth;
        private ITransactionServices _transactionServices;
        public ArticleController(IArticleServices articleServices, IUserAuth userAuth, ITransactionServices transactionServices)
        {
            _articleServices = articleServices;
            _userAuth = userAuth;
            _transactionServices = transactionServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesOverview()
        {
            // return all article titles with description and urls
            var articlesOverview = await _articleServices.GetArticlesOverview();
            return Ok(articlesOverview);
        }
        [Authorize]
        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetOneCompleteArticle(int articleId) //TODO consider removing Correct Answer Index from response because we now have the isCorrect validation for each submitted answer
        {

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Invalid token.");

            var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userIdString);

            if (articleId <= 0) // MySql auto incement ID starts at 1, so 0 or negative ID is invalid
            {
                return BadRequest("Invalid article ID.");
            }
            // return article object of type ArticleModel where ID = acticleId
            var fullArticle = await _articleServices.GetOneCompleteArticleById(articleId);
            if (fullArticle == null)
            {
                return NotFound();
            }
            bool isArticle = true;
            await _transactionServices.CreateTransaction(userIdGuid, articleId, isArticle);
            
            return Ok(fullArticle);
        }

        [Authorize]
        [HttpPost("{articleId}/quiz/submissions")]
        public async Task<IActionResult> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto, int articleId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Invalid token.");

            var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userIdString);

            var isCorrectAnswer = await _articleServices.IsCorrectAnswer(quizQuestionDto.QuestionId ,quizQuestionDto.AnswerSelectionIndex);
            var isLastQuestion = await _articleServices.SubmitOneQuestionAnswer(quizQuestionDto, userIdGuid);

            if (isLastQuestion)
            {
                // TODO add which answer is correct for each question
                bool isArticle = false;
                var correctAnswers = await _articleServices.GetCorrectAnswersForQuiz(articleId);
                await _transactionServices.CreateTransaction(userIdGuid, articleId, isArticle);
                return Ok(new
                {
                    AnswerCorrectStatus = isCorrectAnswer,
                    CorrectAnswers = correctAnswers
                }); // 200: Quiz finished & returns if the answer was correct; Sorry JSON works best here since two different return types are needed (for now)
            }
            else
            {
                // TODO add which answer is correct for each question
                return Accepted("Answer correct status:" + isCorrectAnswer); // 202: Continue to next question & returns if the answer was correct
            }
        }
    }
}
