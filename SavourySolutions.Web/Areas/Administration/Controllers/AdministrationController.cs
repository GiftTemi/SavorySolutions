using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavourySolutions.Common;

namespace SavourySolutions.Web.Areas.Administration.Controllers;

[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
[Area("Administration")]
public class AdministrationController : Controller
{
}
