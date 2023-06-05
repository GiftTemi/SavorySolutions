using System.ComponentModel.DataAnnotations;

using SavourySolutions.Services.Mapping;
using SavourySolutions.Data.Models;

using static SavourySolutions.Models.Common.ModelValidation;
using Ganss.Xss;

namespace SavourySolutions.Models.ViewModels.Categories
{
    public class CategoryDetailsViewModel : IMapFrom<Category>
    {
        [Display(Name = IdDisplayName)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var shortDescription = this.Description;
                return shortDescription.Length > 200
                        ? shortDescription.Substring(0, 200) + " ..."
                        : shortDescription;
            }
        }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string SanitizedShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public string UserUsername { get; set; }
    }
}
