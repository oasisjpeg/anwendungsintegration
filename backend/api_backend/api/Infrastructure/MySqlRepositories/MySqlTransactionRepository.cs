using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories
{
    public class MySqlTransactionRepository : ITransactionRepository
    {
        public Task<List<RewardTransactionModel>> GetTransactionsFromDb()
        {
            throw new NotImplementedException();
        }
    }
}
