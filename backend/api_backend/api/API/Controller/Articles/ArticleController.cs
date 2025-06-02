using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Application_Layer.Services.Article;
using WebApplication1.Application_Layer.Services.Transaction;
using WebApplication1.Application_Layer.Services.UserAuth;

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

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetOneCompleteArticle(int articleId)
        {
            if(articleId <= 0) // MySql auto incement ID starts at 1, so 0 or negative ID is invalid
            {
                return BadRequest("Invalid article ID.");
            }
            // return article object of type ArticleModel where ID = acticleId
            var fullArticle = await _articleServices.GetOneCompleteArticleById(articleId);
            if (fullArticle == null)
            {
                return NotFound();
            }
            // TODO: call createTransaction method to get points for reading article
            var rewardPointsGained = await _transactionServices.CalculateArticlePoints(articleId);

            return Ok(fullArticle);
        }

        [Authorize]
        [HttpPost("{id}/quiz/submissions")]
        public async Task<IActionResult> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto)
        {
            // read userId from token claims
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("Invalid token.");

            var userIdGuid = _userAuth.GetUserIdGuidFromClaims(userIdString);

            // save one question answer to database 
            // return 202 if not last question --> check if last question in quiz for each answer submission
            // return 200 if last question, to inform front end that quiz is now finished, aka the next question is the last question
            var submitAndCheckLastQuestion = await _articleServices.SubmitOneQuestionAnswer(quizQuestionDto, userIdGuid);

            return Ok(submitAndCheckLastQuestion);
        }
    }
}
