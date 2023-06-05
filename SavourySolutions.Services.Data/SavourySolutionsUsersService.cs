namespace SavourySolutions.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SavourySolutions.Data.Common.Repositories;
    using SavourySolutions.Data.Models;
    using SavourySolutions.Services.Data.Common;
    using SavourySolutions.Services.Data.Contracts;
    using SavourySolutions.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class SavourySolutionsUsersService : ISavourySolutionsUsersService
    {
        private readonly IDeletableEntityRepository<SavourySolutionsUser> SavourySolutionsUsersRepository;

        public SavourySolutionsUsersService(IDeletableEntityRepository<SavourySolutionsUser> SavourySolutionsUsersRepository)
        {
            this.SavourySolutionsUsersRepository = SavourySolutionsUsersRepository;
        }

        public async Task BanByIdAsync(string id)
        {
            var SavourySolutionsUser = await this.SavourySolutionsUsersRepository
                .All()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (SavourySolutionsUser == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.SavourySolutionsUserNotFound, id));
            }

            this.SavourySolutionsUsersRepository.Delete(SavourySolutionsUser);
            await this.SavourySolutionsUsersRepository.SaveChangesAsync();
        }

        public async Task UnbanByIdAsync(string id)
        {
            var SavourySolutionsUser = await this.SavourySolutionsUsersRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (SavourySolutionsUser == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.SavourySolutionsUserNotFound, id));
            }

            this.SavourySolutionsUsersRepository.Undelete(SavourySolutionsUser);
            await this.SavourySolutionsUsersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TViewModel>> GetAllSavourySolutionsUsersAsync<TViewModel>()
        {
            var users = await this.SavourySolutionsUsersRepository
              .AllWithDeleted()
              .To<TViewModel>()
              .ToListAsync();

            return users;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(string id)
        {
            var SavourySolutionsUserViewModel = await this.SavourySolutionsUsersRepository
                .AllWithDeleted()
                .Where(u => u.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (SavourySolutionsUserViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.SavourySolutionsUserNotFound, id));
            }

            return SavourySolutionsUserViewModel;
        }
    }
}
