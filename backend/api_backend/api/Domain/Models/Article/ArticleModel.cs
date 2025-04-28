namespace WebApplication1.Domain.Models.Article
{
    public class ArticleModel
    {
        public required int ArticleId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string[] URL { get; set; }
        public required DateTime DateTime { get; set; }

    }
}
