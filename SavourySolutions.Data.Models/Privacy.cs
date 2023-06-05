using System.ComponentModel.DataAnnotations;
using SavourySolutions.Data.Common.Models;
using static SavourySolutions.Data.Common.DataValidation.PrivacyValidation;

namespace SavourySolutions.Data.Models;
public class Privacy : BaseDeletableModel<int>
{
    [Required]
    [MaxLength(ContentPageMaxLength)]
    public string PageContent { get; set; }
}
