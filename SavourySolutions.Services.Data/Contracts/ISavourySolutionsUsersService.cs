namespace SavourySolutions.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISavourySolutionsUsersService
    {
        Task BanByIdAsync(string id);

        Task UnbanByIdAsync(string id);

        Task<IEnumerable<TViewModel>> GetAllSavourySolutionsUsersAsync<TViewModel>();

        Task<TViewModel> GetViewModelByIdAsync<TViewModel>(string id);
    }
}
