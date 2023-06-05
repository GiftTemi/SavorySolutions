namespace SavourySolutions.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SavourySolutions.Data.Common.Models;

    using static SavourySolutions.Data.Common.DataValidation.ReviewCommentValidation;

    public class ReviewComment : BaseDeletableModel<int>
    {
        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }

        public int? ParentId { get; set; }

        public virtual ReviewComment Parent { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual SavourySolutionsUser User { get; set; }
    }
}
