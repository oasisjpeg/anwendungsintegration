using WebApplication1.Application_Layer.DTO.Article;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Repositories
{
    public interface IArticleRepository
    {
        Task<int> CreateTransaction(PointSourceType pointSourceType, int sourceId, Guid userId);
        Task<List<ArticleOverviewDto>> GetArticleOverviewFromDB();
        Task<CompleteArticleDto> GetOneCompleteArticleFromDB(int articleId);
        Task SubmitOneQuestionAnswerToDB(QuizQuestionDto quizQuestionDto, Guid userId);
        Task<bool> IsLastQuestionInQuiz(int questionId);
    }
}
