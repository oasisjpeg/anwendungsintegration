using WebApplication1.Domain.Models.User;

namespace WebApplication1.Application_Layer.Services.Transaction
{
    public interface ITransactionServices
    {
        Task CreateTransaction(Guid userId, int articleId, bool isArticle);
        Task<int> CalculateArticlePoints(int? articleId);
        Task<bool> PreventDuplicateTransaction(Guid userId, PointSourceType sourceType, int articleId);
    }
}
