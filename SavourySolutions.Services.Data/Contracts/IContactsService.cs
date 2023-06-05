namespace SavourySolutions.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using SavourySolutions.Models.ViewModels.Contacts;

    public interface IContactsService
    {
        Task SendContactToAdminAsync(ContactFormEntryViewModel contactFormEntryViewModel);
    }
}
