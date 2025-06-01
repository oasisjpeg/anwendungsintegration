using WebApplication1.Application_Layer.DTO.Article;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Application_Layer.Services.Article
{
    public interface IArticleServices
    {
        Task<int> CreateTransaction(Enum sourceType, int sourceId, Guid userId); // return point amount AND either change name to "CreateArticleTransaction" or create in TransactionServices
        Task<List<ArticleOverviewDto>> GetArticlesOverview();
        Task<ArticleModel> GetOneCompleteArticleById(int id);
        Task<bool> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto, Guid userId);
    }
}
