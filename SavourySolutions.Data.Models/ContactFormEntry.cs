namespace SavourySolutions.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using SavourySolutions.Data.Common.Models;

    using static SavourySolutions.Data.Common.DataValidation;
    using static SavourySolutions.Data.Common.DataValidation.ContactFormEntryValidation;

    public class ContactFormEntry : BaseModel<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(SubjectMaxLength)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }
    }
}
