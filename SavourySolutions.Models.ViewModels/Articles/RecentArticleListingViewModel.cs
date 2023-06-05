﻿namespace SavourySolutions.Models.ViewModels.Articles
{
    using System;

    using SavourySolutions.Data.Models;
    using SavourySolutions.Services.Mapping;

    public class RecentArticleListingViewModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
