using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Domain.Models.User
{
    public class UserAnswerModel
    {
        public required int id { get; set; }
        public required DateTime AnsweredAt { get; set; }
        public required int SelectedAnswer {  get; set; }
        // foreign keys
        public required Guid UserId { get; set; }
        public required int QuestionId { get; set; }
        // navigation props
        public UserModel User { get; set; }
        public required QuestionModel Question { get; set; }

    }
}
