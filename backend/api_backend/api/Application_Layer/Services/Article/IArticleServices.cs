using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Application_Layer.Services.Article
{
    public interface IArticleServices
    {
        Task<int> CreateTransaction(Enum sourceType, int sourceId, string userId); // return point amount
        Task<List<ArticleModel>> GetArticlesOverview();
        Task<ArticleModel> GetOneCompleteArticleById(int id);
        Task<bool> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto, Guid userId);
    }
}
