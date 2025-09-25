using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Application_Layer.DTO.Quiz
{
    public class QuizQuestionDto
    {
        [Key]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "QuestionId must be greater than 0.")]
        public required int QuestionId { get; set; }

        [Required]
        [Range(0, 3)]
        public required int AnswerSelectionIndex { get; set; } // 0 - 3 answer index options
    }
}
