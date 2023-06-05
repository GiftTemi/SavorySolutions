namespace SavourySolutions.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SavourySolutions.Data.Common.Models;

    using static SavourySolutions.Data.Common.DataValidation.FaqEntryValidation;

    public class FaqEntry : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(QuestionMaxLength)]
        public string Question { get; set; }

        [Required]
        [MaxLength(AnswerMaxLength)]
        public string Answer { get; set; }
    }
}
