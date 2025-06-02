using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.Services.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        private ITransactionRepository _transactionRepository;
        public TransactionServices(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<int> CalculateArticlePoints(int articleId)
        {
            var articlePoints = await _transactionRepository.GetArticlePoints(articleId);
            return articlePoints;
        }
    }
}
