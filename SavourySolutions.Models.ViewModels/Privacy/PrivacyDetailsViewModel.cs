using System.ComponentModel.DataAnnotations;
using SavourySolutions.Services.Mapping;
using SavourySolutions.Data.Models;
using static SavourySolutions.Models.Common.ModelValidation.PrivacyValidation;
using Ganss.Xss;

namespace SavourySolutions.Models.ViewModels.Privacy;
public class PrivacyDetailsViewModel : IMapFrom<SavourySolutions.Data.Models.Privacy>
{
    public int Id { get; set; }

    [Display(Name = PageContentDisplayName)]
    public string PageContent { get; set; }

    public string SanitizedPageContent => new HtmlSanitizer().Sanitize(this.PageContent);
}
