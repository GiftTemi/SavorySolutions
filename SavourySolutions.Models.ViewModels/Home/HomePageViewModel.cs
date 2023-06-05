namespace SavourySolutions.Models.ViewModels.Home
{
    using System.Collections.Generic;

    using SavourySolutions.Models.ViewModels.Articles;
    using SavourySolutions.Models.ViewModels.Recipes;

    public class HomePageViewModel
    {
        public IEnumerable<ArticleListingViewModel> RecentArticles { get; set; }

        public IEnumerable<RecipeListingViewModel> TopRecipes { get; set; }

        public IEnumerable<GalleryViewModel> Gallery { get; set; }
    }
}
