namespace SavourySolutions.Models.ViewModels.Privacy
{
    using System.ComponentModel.DataAnnotations;

    using SavourySolutions.Data.Models;
    using SavourySolutions.Services.Mapping;

    using static SavourySolutions.Models.Common.ModelValidation;
    using static SavourySolutions.Models.Common.ModelValidation.PrivacyValidation;

    public class PrivacyEditViewModel : IMapFrom<Privacy>
    {
        public int Id { get; set; }

        [Display(Name = PageContentDisplayName)]
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(PageContentMaxLength, MinimumLength = PageContentMinLength, ErrorMessage = PageContentLengthError)]
        public string PageContent { get; set; }
    }
}
