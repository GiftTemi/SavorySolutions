namespace SavourySolutions.Models.ViewModels.AdminDashboard
{
    using SavourySolutions.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class DashboardContentModel
    {
        public int RegisteredUsers { get; set; }

        public int UsersToday { get; set; }

        public int Admins { get; set; }

        public int RecipesCount {get;set;}

        public int ArticlesCount { get; set; }

        public int ReviewsCount { get; set; }

        public int ArticleCommentsCount { get; set; }

        public int CategoriesCount { get; set; }

        public List<Category> TopCategories { get; set; }

        public List<SavourySolutionsUser> TopUsers { get; set; }

    }
}
