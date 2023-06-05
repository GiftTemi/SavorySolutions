
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.Rendering;
using static SavourySolutions.Models.Common.ModelValidation.SavourySolutionsUserValidation;

namespace SavourySolutions.Models.ViewModels.SavourySolutionsUsers;

public class SavourySolutionsUserEditViewModel
{
    public string RoleId { get; set; }

    public string RoleName { get; set; }

    [Required(ErrorMessage = RoleSelectedError)]
    public string NewRole { get; set; }

    public List<SelectListItem> RolesList { get; set; }
}
