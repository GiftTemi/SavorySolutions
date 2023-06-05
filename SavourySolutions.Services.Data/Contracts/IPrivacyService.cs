namespace SavourySolutions.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using SavourySolutions.Models.InputModels.AdministratorInputModels.Privacy;
    using SavourySolutions.Models.ViewModels.Privacy;

    public interface IPrivacyService : IBaseDataService
    {
        Task<PrivacyDetailsViewModel> CreateAsync(PrivacyCreateInputModel privacyCreateInputModel);

        Task EditAsync(PrivacyEditViewModel privacyEditViewModel);

        Task<TViewModel> GetViewModelAsync<TViewModel>();
    }
}
