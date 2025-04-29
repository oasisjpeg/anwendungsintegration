using WebApplication1.Domain.Models.User;

namespace WebApplication1.Application_Layer.Services.Transaction
{
    public interface ITransactionServices
    {
        Task<List<RewardTransactionModel>> GetTransactions();
    }
}
