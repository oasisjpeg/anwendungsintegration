namespace WebApplication1.Application_Layer.DTO.Article
{
    public class ArticleOverviewDto
    {
        // i think every article should have a Title, Description and at least one Url --> so all required
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string[] Url { get; set; }
    }
}
