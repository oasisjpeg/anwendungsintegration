using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<int> GetArticlePoints(int? articleId);
        Task CreateTransaction(PointSourceType sourceType, int pointSourceId, Guid userId, int pointAmount);
        Task<bool> PreventDuplicateTransaction(Guid userId, PointSourceType sourceType, int articleId);
    }
}
