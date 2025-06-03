using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Domain.Models.Article
{
    public class QuizModel
    {
        public required int id { get; set; }
        public required string Title { get; set; }
        // fk
        public required int ArticleId { get; set; }
        // navigation property (not required to prevent circular references)
        public ArticleModel? Article { get; set; }
        
        public ICollection<QuestionModel> Questions { get; set; }

    }
}
