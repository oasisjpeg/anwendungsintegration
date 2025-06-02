namespace WebApplication1.Domain.Models.Article
{
    public class ArticleModel
    {
        public required int ArticleId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string[] Url { get; set; } // note to self, variable names are case-sensitive...? lol
        // NOTE: we might have to change to different format for EF Core to handle arrays properly, like using a JSON column type in MySQL
        public required DateTime DateTime { get; set; }
        public required string Description { get; set; }
    }
}