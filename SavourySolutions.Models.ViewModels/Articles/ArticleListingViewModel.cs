using SavourySolutions.Data.Models;
using SavourySolutions.Services.Mapping;
using SavourySolutions.Models.ViewModels.ArticleComments;
using Ganss.Xss;

namespace SavourySolutions.Models.ViewModels.Articles
{
    public class ArticleListingViewModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ShortDescription
        {
            get
            {
                var shortDescription = this.Description;
                return shortDescription.Length > 100
                        ? shortDescription.Substring(0, 100) + " ..."
                        : shortDescription;
            }
        }

        public string SanitizedShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<PostArticleCommentViewModel> ArticleComments { get; set; }
    }
}