using WebApplication1.Domain.Models.User;

namespace WebApplication1.Application_Layer.Services.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        public Task<List<RewardTransactionModel>> GetTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
