using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories
{
    public class MySqlTransactionRepository : ITransactionRepository
    {
        private readonly MySqlDbContext _context;
        public MySqlTransactionRepository(MySqlDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetArticlePoints(int articleId)
        {
            var articleContent = await _context.Articles
                .Where(a => a.id == articleId)
                .Select(a => a.Content)
                .FirstOrDefaultAsync();

            if (articleContent == null)
            {
                throw new DirectoryNotFoundException();
            }

            var wordCount = articleContent.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            var points = (int)Math.Round(0.2 * wordCount);
            // TODO: add point max
            if (points > 300)
            {
                points = 300;
            }

            return points;
        }
    }
}
