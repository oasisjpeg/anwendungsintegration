using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain.Models.Article;

namespace WebApplication1.Application_Layer.DTO.Quiz
{
    public class QuizQuestionDto
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public required int QuestionId { get; set; }

        [Required]
        [MaxLength(100)]
        public int QuizId { get; set; }

        [Required]
        [MaxLength(1)]
        public required int AnswerSelectionIndex { get; set; } // 0 - 3 answer index options
    }
}
