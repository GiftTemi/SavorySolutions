namespace SavourySolutions.Models.ViewModels.Recipes
{
    using SavourySolutions.Data.Models;
    using SavourySolutions.Services.Mapping;

    public class GalleryViewModel : IMapFrom<Recipe>
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }
    }
}
