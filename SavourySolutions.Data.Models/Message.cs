namespace SavourySolutions.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SavourySolutions.Data.Common.Models;

    using static SavourySolutions.Data.Common.DataValidation.MessageValidation;

    public class Message : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public virtual SavourySolutionsUser User { get; set; }
    }
}
