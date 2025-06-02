namespace WebApplication1.Domain.Models.Article
{
    public class QuestionModel
    {
        public required int QuestionId { get; set; }
        public required string QuestionText { get; set; }
        public required string FirstAnswerOption { get; set; }
        public required string SecondAnswerOption { get; set; }
        public required string ThirdAnswerOption { get; set; }
        public required string FourthAnswerOption { get; set; }
        public required int CorrectAnswerIndex { get; set; }
        // FK
        public required int QuizId { get; set; }

        // navigation prop
        public required QuizModel Quiz { get; set; }
    }
}
