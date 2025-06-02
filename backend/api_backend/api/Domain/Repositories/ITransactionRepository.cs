using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<int> GetArticlePoints(int articleId);
    }
}
