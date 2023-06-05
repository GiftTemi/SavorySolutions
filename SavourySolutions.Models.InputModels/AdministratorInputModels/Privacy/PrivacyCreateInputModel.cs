namespace SavourySolutions.Models.InputModels.AdministratorInputModels.Privacy
{
    using System.ComponentModel.DataAnnotations;

    using static SavourySolutions.Models.Common.ModelValidation;
    using static SavourySolutions.Models.Common.ModelValidation.PrivacyValidation;

    public class PrivacyCreateInputModel
    {
        [Display(Name = PageContentDisplayName)]
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(PageContentMaxLength, MinimumLength = PageContentMinLength, ErrorMessage = PageContentLengthError)]
        public string PageContent { get; set; }
    }
}
