namespace SavourySolutions.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SavourySolutions.Models.InputModels.AdministratorInputModels.Faq;
    using SavourySolutions.Models.ViewModels.Faq;

    public interface IFaqService : IBaseDataService
    {
        Task<FaqDetailsViewModel> CreateAsync(FaqCreateInputModel faqCreateInputModel);

        Task EditAsync(FaqEditViewModel faqEditViewModel);

        Task<IEnumerable<TEntity>> GetAllFaqsAsync<TEntity>();
    }
}
