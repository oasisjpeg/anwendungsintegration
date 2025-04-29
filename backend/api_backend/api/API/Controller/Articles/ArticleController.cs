using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application_Layer.DTO.Quiz;

namespace WebApplication1.API.Controller.Articles
{
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetArticlesOverview()
        {
            // return all article titles with description and urls
            return null;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneCompleteArticle(int arcticleId)
        {
            // return article object of type ArticleModel where ID = acticleId 
            return null;
        }

        [HttpPost("{id}/quiz/submissions")]
        public async Task<IActionResult> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto)
        {
            // save one question answer to database 
            // return 202 if not last question --> check if last question in quiz for each answer submission
            // return 200 if last question, to inform front end that quiz is now finished, aka the next question is the last question
            return null;
        }
    }
}
