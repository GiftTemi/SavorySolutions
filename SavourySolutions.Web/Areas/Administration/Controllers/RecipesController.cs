namespace SavourySolutions.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using SavourySolutions.Data.Models;
    using SavourySolutions.Models.InputModels.AdministratorInputModels.Recipes;
    using SavourySolutions.Models.ViewModels;
    using SavourySolutions.Models.ViewModels.Categories;
    using SavourySolutions.Models.ViewModels.Recipes;
    using SavourySolutions.Services.Data.Contracts;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : AdministrationController
    {
        private const int PageSize = 6;
        private readonly IRecipesService recipesService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<SavourySolutionsUser> userManager;

        public RecipesController(
            IRecipesService recipesService,
            ICategoriesService categoriesService,
            UserManager<SavourySolutionsUser> userManager)
        {
            this.recipesService = recipesService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var categories = await this.categoriesService
                .GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var model = new RecipeCreateInputModel
            {
                Categories = categories,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeCreateInputModel recipeCreateInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService
                  .GetAllCategoriesAsync<CategoryDetailsViewModel>();

                recipeCreateInputModel.Categories = categories;

                return this.View(recipeCreateInputModel);
            }

            await this.recipesService.CreateAsync(recipeCreateInputModel, user.Id);
            return this.RedirectToAction("GetAll", "Recipes", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var recipeToEdit = await this.recipesService
                .GetViewModelByIdAsync<RecipeEditViewModel>(id);

            var categories = await this.categoriesService
                  .GetAllCategoriesAsync<CategoryDetailsViewModel>();

            recipeToEdit.Categories = categories;

            return this.View(recipeToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecipeEditViewModel recipeEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(recipeEditViewModel);
            }

            await this.recipesService.EditAsync(recipeEditViewModel);
            return this.RedirectToAction("GetAll", "Recipes", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var recipeToDelete = await this.recipesService.GetViewModelByIdAsync<RecipeDetailsViewModel>(id);

            return this.View(recipeToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(RecipeDetailsViewModel recipesDetailsViewModel)
        {
            await this.recipesService.DeleteByIdAsync(recipesDetailsViewModel.Id);

            return this.RedirectToAction("GetAll", "Recipes", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll(int? pageNumber)
        {
            var recipes = this.recipesService
                .GetAllRecipesAsQueryeable<RecipeDetailsViewModel>();

            var recipesPaginated = await PaginatedList<RecipeDetailsViewModel>
                .CreateAsync(recipes, pageNumber ?? 1, PageSize);

            return this.View(recipesPaginated);
        }
    }
}
