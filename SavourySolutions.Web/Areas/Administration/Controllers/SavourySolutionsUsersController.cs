
using SavourySolutions.Data.Models;
using SavourySolutions.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SavourySolutions.Common;
using SavourySolutions.Models.ViewModels.SavourySolutionsUsers;

namespace SavourySolutions.Web.Areas.Administration.Controllers
{

    public class SavourySolutionsUsersController : AdministrationController
    {
        private readonly UserManager<SavourySolutionsUser> SavourySolutionsUserManager;
        private readonly ISavourySolutionsUsersService SavourySolutionsUsersService;
        private readonly RoleManager<ApplicationRole> roleManager;

        public SavourySolutionsUsersController(
            UserManager<SavourySolutionsUser> SavourySolutionsUserManager,
            ISavourySolutionsUsersService SavourySolutionsUsersService,
            RoleManager<ApplicationRole> roleManager)
        {
            this.SavourySolutionsUserManager = SavourySolutionsUserManager;
            this.SavourySolutionsUsersService = SavourySolutionsUsersService;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAll()
        {
            var users = await this.SavourySolutionsUsersService
                .GetAllSavourySolutionsUsersAsync<SavourySolutionsUserDetailsViewModel>();                                                 
            return this.View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.SavourySolutionsUserManager.FindByIdAsync(id);
            var isAdmin = await this.SavourySolutionsUserManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);
            var isUser = await this.SavourySolutionsUserManager.IsInRoleAsync(user, GlobalConstants.UserRoleName);

            var currUserRole = user.Roles.FirstOrDefault(x => x.UserId == id);
            var currUserRoleName = await this.roleManager.FindByIdAsync(currUserRole.RoleId);

            var viewModel = new SavourySolutionsUserEditViewModel
            {
                RoleId = currUserRole.RoleId,
                RoleName = currUserRoleName.Name,
            };

            viewModel.RolesList = this.roleManager.Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                })
                .ToList();

            if (currUserRoleName.Name == GlobalConstants.AdministratorRoleName && isAdmin == true)
            {
                viewModel.RolesList
                    .Find(x => x.Text == GlobalConstants.AdministratorRoleName).Selected = true;
            }

            if (currUserRoleName.Name == GlobalConstants.UserRoleName && isUser == true)
            {
                viewModel.RolesList
                    .Find(x => x.Text == GlobalConstants.UserRoleName).Selected = true;
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavourySolutionsUserEditViewModel model, string id)
        {
            if (!this.ModelState.IsValid)
            {
                model.RolesList = this.roleManager.Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                }).ToList();
                return this.View(model);
            }

            if (model.NewRole == model.RoleName)
            {
                model.RolesList = this.roleManager.Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                })
                .ToList();

                return this.View(model);
            }

            var user = await this.SavourySolutionsUserManager.FindByIdAsync(id);

            await this.SavourySolutionsUserManager.RemoveFromRoleAsync(user, model.RoleName);

            await this.SavourySolutionsUserManager.AddToRoleAsync(
                user,
                model.NewRole);

            return this.RedirectToAction("GetAll", "SavourySolutionsUsers", new { area = "Administration" });
        }

        public async Task<IActionResult> Ban(string id)
        {
            var SavourySolutionsUserToBan = await this.SavourySolutionsUsersService
                .GetViewModelByIdAsync<SavourySolutionsUserDetailsViewModel>(id);

            return this.View(SavourySolutionsUserToBan);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(SavourySolutionsUserDetailsViewModel SavourySolutionsUserDetailsViewModel)
        {
            await this.SavourySolutionsUsersService.BanByIdAsync(SavourySolutionsUserDetailsViewModel.Id);

            return this.RedirectToAction("GetAll", "SavourySolutionsUsers", new { area = "Administration" });
        }

        public async Task<IActionResult> Unban(string id)
        {
            var SavourySolutionsUserToUnban = await this.SavourySolutionsUsersService
                .GetViewModelByIdAsync<SavourySolutionsUserDetailsViewModel>(id);

            return this.View(SavourySolutionsUserToUnban);
        }

        [HttpPost]
        public async Task<IActionResult> Unban(SavourySolutionsUserDetailsViewModel SavourySolutionsUserDetailsViewModel)
        {
            await this.SavourySolutionsUsersService.UnbanByIdAsync(SavourySolutionsUserDetailsViewModel.Id);

            return this.RedirectToAction("GetAll", "SavourySolutionsUsers", new { area = "Administration" });
        }
    }
}
