using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Application_Layer.DTO.Article;

public class CompleteArticleDto
{
        public ArticleModel Article { get; set; }
        public QuizModel Quiz { get; set; }
}