using Ganss.Xss;
using SavourySolutions.Data.Models;
using SavourySolutions.Services.Mapping;

namespace SavourySolutions.Models.ViewModels.ArticleComments
{
    public class PostArticleCommentViewModel : IMapFrom<ArticleComment>
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }
    }
}
