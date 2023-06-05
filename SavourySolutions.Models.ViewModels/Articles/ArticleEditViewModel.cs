namespace SavourySolutions.Models.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SavourySolutions.Services.Mapping;
    using SavourySolutions.Data.Models;
    using SavourySolutions.Models.ViewModels.Categories;

    using Microsoft.AspNetCore.Http;

    using static SavourySolutions.Models.Common.ModelValidation;
    using static SavourySolutions.Models.Common.ModelValidation.ArticleValidation;

    public class ArticleEditViewModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthError)]
        public string Title { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionLengthError)]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        [StringLength(ImageMaxLength, MinimumLength = ImageMinLength, ErrorMessage = ImagePathError)]
        public string ImagePath { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Display(Name = nameof(Category))]
        public int CategoryId { get; set; }
        
        public IEnumerable<CategoryDetailsViewModel> Categories { get; set; }
    }
}
