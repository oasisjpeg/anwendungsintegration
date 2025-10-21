using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Application_Layer.DTO.Article;

public class CompleteArticleDto
{
        public required ArticleModel Article { get; set; }
        public required QuizModel Quiz { get; set; }
}