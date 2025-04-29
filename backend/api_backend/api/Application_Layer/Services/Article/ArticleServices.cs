using System.Transactions;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Application_Layer.Services.Article
{
    public class ArticleServices : IArticleServices
    {
        public Task<int> CreateTransaction(Enum sourceType, int sourceId, Guid userId)
        {
            var newTransaction = new RewardTransactionModel
            {
                TransactionId = 0,
                Created = DateTime.Now,
                PointsGained = 0, // <-- Calculate Points
                PointSourceType = sourceType,
                PointSourceId = sourceId,
                UserId = userId,
                User = GetUserById(userId)
            };
            int pointsGained = newTransaction.PointsGained;
            return pointsGained;
        }

        public Task<List<ArticleModel>> GetArticlesOverview()
        {
            throw new NotImplementedException();
        }

        public Task<ArticleModel> GetOneCompleteArticleById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubmitOneQuestionAnswer(QuizQuestionDto quizQuestionDto, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
