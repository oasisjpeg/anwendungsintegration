using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Application_Layer.DTO.Quiz
{
    public class QuizQuestionDto
    {
        [Required]
        [MaxLength(10)]
        public required int QuestionId { get; set; }
        [Required]
        [MaxLength(10)]
        public required int AnswerSelectionIndex { get; set; } // 0 - 3 answer index options
    }
}
