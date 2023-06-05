namespace SavourySolutions.Models.ViewModels.Categories
{
    using System.Collections.Generic;

    using SavourySolutions.Data.Models;
    using SavourySolutions.Models.ViewModels.Articles;
    using SavourySolutions.Services.Mapping;

    public class CategoryListingViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<RecentArticleListingViewModel> Articles { get; set; }
    }
}
