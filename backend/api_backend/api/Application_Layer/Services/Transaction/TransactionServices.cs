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
        public async Task CreateTransaction(Guid userId, int articleId, bool isArticle)
        {
            var userModel = await _userRepository.GetByIdAsync(userId);

            if (userModel == null)
            {
                throw new Exception("User not found.");
            }
            int pointAmount;
            if (isArticle == true)
            {
                PointSourceType pointSourceType = PointSourceType.Article;
                var isDuplicate = await PreventDuplicateTransaction(userId, pointSourceType, articleId);
                if (isDuplicate)
                {
                    return; // no need to create a new transaction with zero points...
                }
                else
                {
                    pointAmount = await CalculateArticlePoints(articleId);
                }
                await _transactionRepository.CreateTransaction(pointSourceType, articleId, userId, pointAmount);
            }
            else
            {
                PointSourceType pointSourceType = PointSourceType.Quiz;
                var isDuplicate = await PreventDuplicateTransaction(userId, pointSourceType, articleId);
                if (isDuplicate)
                {
                    return; // no need to create a new transaction with zero points...
                }
                else
                {
                    pointAmount = 15; // all quizes reward 15 points upon completion
                }
                await _transactionRepository.CreateTransaction(pointSourceType, articleId, userId, pointAmount);
            }
        }

        public Task<bool> PreventDuplicateTransaction(Guid userId, PointSourceType sourceType, int articleId)
        {
            var isDuplicate = _transactionRepository.PreventDuplicateTransaction(userId, sourceType, articleId);
            return isDuplicate;
        }
    }
}
