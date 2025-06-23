namespace WebApplication1.Domain.Models.Article
{
    public class ArticleModel
    {
        public required int id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required List<string> Url { get; set; } // note to self, variable names are case-sensitive...
        public required DateTime DateTime { get; set; }
        public required string Description { get; set; }
    }
}