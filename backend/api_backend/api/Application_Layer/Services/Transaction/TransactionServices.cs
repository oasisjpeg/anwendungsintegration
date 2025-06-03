using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.Services.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        public TransactionServices(ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }
        public async Task<int> CalculateArticlePoints(int? articleId)
        {
            var articlePoints = await _transactionRepository.GetArticlePoints(articleId);
            return articlePoints;
        }
        public async Task CreateTransaction(Guid userId, int? articleId)
        {
            var userModel = await _userRepository.GetByIdAsync(userId);

            if (userModel == null)
            {
                throw new Exception("User not found.");
            }
            int pointAmount;
            if (articleId == null)
            {
                PointSourceType pointSourceType = PointSourceType.Article;
                pointAmount = await CalculateArticlePoints(articleId);
                await _transactionRepository.CreateTransaction(pointSourceType, userId, pointAmount);
            }
            else
            {
                PointSourceType pointSourceType = PointSourceType.Quiz;
                pointAmount = 15; // all quizes reward 15 points upon completion
                await _transactionRepository.CreateTransaction(pointSourceType, userId, pointAmount);
            }
        }
    }
}
