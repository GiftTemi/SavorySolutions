namespace SavourySolutions.Models.InputModels.AdministratorInputModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using static SavourySolutions.Models.Common.ModelValidation;
    using static SavourySolutions.Models.Common.ModelValidation.CategoryValidation;

    public class CategoryCreateInputModel
    {
        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthError)]
        public string Name { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionError)]
        public string Description { get; set; }
    }
}
