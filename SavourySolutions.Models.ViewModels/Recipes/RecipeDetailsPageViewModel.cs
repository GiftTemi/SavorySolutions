namespace SavourySolutions.Models.ViewModels.Recipes
{
    using SavourySolutions.Models.ViewModels.Reviews;

    public class RecipeDetailsPageViewModel
    {
        public RecipeDetailsViewModel Recipe { get; set; }

        public CreateReviewInputModel CreateReviewInputModel { get; set; }
    }
}
