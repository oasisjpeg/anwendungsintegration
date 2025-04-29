using Microsoft.IdentityModel.Tokens;
using System.Transactions;
using WebApplication1.Application_Layer.DTO.Quiz;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.Services.Article
{
    public class ArticleServices : IArticleServices
    {
        private IUserRepository _userRepository;

        public ArticleServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<int> CreateTransaction(Enum sourceType, int sourceId, string userId)
        {
            var userModel = _userRepository.GetByIdAsync(userId);

            if(userModel== null)
            {
                return null;
            }

            var newTransaction = new RewardTransactionModel
            {
                TransactionId = 0,
                Created = DateTime.Now,
                PointsGained = 0, // <-- Calculate Points
                PointSourceType = sourceType,
                PointSourceId = sourceId,
                UserId = userId, // implicit conversion not possible --> change GUID to STRING
                User = userModel // implicit conversion not possible --> check out userModel 
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
