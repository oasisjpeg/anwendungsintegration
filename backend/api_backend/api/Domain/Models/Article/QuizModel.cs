using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Domain.Models.Article
{
    public class QuizModel
    {
        public required int id { get; set; }
        public required string Title { get; set; }
        // fk
        public required int ArticleId { get; set; }
        // navigation property
        public required ArticleModel Article { get; set; }
    }
}
