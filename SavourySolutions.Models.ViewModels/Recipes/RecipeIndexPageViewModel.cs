namespace SavourySolutions.Models.ViewModels.Recipes
{
    using System.Collections.Generic;

    using SavourySolutions.Models.ViewModels.Categories;
    using SavourySolutions.Models.ViewModels.Reviews;

    public class RecipeIndexPageViewModel
    {
        public PaginatedList<RecipeListingViewModel> RecipesPaginated { get; set; }

        public IEnumerable<CategoryListingViewModel> Categories { get; set; }

        public IEnumerable<ReviewListingViewModel> Reviews { get; set; }
    }
}
