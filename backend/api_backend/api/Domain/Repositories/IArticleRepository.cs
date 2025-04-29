using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Domain.Repositories
{
    public interface IArticleRepository
    {
        Task<List<ArticleModel>> GetArticleOverviewFromDB();
        Task<ArticleModel> GetOneCompleteArticleFromDB(int articleId);
        Task SubmitOneQuestionAnswerFromDB(QuizQuestionDto quizQuestionDto, Guid userId);
    }
}
