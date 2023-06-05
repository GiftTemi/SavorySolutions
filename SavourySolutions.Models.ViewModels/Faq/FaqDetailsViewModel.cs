using Ganss.Xss;
using SavourySolutions.Data.Models;
using SavourySolutions.Services.Mapping;

namespace SavourySolutions.Models.ViewModels.Faq
{

    public class FaqDetailsViewModel : IMapFrom<FaqEntry>
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string SanitizedAnswer => new HtmlSanitizer().Sanitize(this.Answer);
    }
}
