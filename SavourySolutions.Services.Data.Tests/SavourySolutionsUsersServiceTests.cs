namespace SavourySolutions.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using SavourySolutions.Data;
    using SavourySolutions.Data.Models;
    using SavourySolutions.Data.Models.Enumerations;
    using SavourySolutions.Data.Repositories;
    using SavourySolutions.Services.Data.Common;
    using SavourySolutions.Services.Data.Contracts;
    using SavourySolutions.Services.Mapping;
    using SavourySolutions.Models.ViewModels.SavourySolutionsUsers;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using Xunit;

    public class SavourySolutionsUsersServiceTests : IAsyncDisposable
    {
        private readonly ISavourySolutionsUsersService SavourySolutionsUsersService;
        private EfDeletableEntityRepository<SavourySolutionsUser> SavourySolutionsUsersRepository;
        private SqliteConnection connection;

        private SavourySolutionsUser firstSavourySolutionsUser;

        public SavourySolutionsUsersServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.SavourySolutionsUsersService = new SavourySolutionsUsersService(this.SavourySolutionsUsersRepository);
        }

        [Fact]
        public async Task CheckIfBanByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            await this.SavourySolutionsUsersService.BanByIdAsync(this.firstSavourySolutionsUser.Id);

            var count = await this.SavourySolutionsUsersRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfBanByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.SavourySolutionsUsersService.BanByIdAsync("3"));

            Assert.Equal(string.Format(ExceptionMessages.SavourySolutionsUserNotFound, 3), exception.Message);
        }

        [Fact]
        public async Task CheckIfUnbanByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            await this.SavourySolutionsUsersService.UnbanByIdAsync(this.firstSavourySolutionsUser.Id);

            var count = await this.SavourySolutionsUsersRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfUnbanByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.SavourySolutionsUsersService.UnbanByIdAsync("2"));

            Assert.Equal(string.Format(ExceptionMessages.SavourySolutionsUserNotFound, 2), exception.Message);
        }

        [Fact]
        public async Task CheckIfGetAllSavourySolutionsUsersAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.SavourySolutionsUsersService.GetAllSavourySolutionsUsersAsync<SavourySolutionsUserEditViewModel>();

            var count = result.Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new SavourySolutionsUserDetailsViewModel
            {
                Id = this.firstSavourySolutionsUser.Id,
                Username = this.firstSavourySolutionsUser.UserName,
                FullName = this.firstSavourySolutionsUser.FullName,
                CreatedOn = this.firstSavourySolutionsUser.CreatedOn,
                isDeleted = this.firstSavourySolutionsUser.IsDeleted,
                Gender = this.firstSavourySolutionsUser.Gender,
            };

            var viewModel = await this.SavourySolutionsUsersService
                .GetViewModelByIdAsync<SavourySolutionsUserEditViewModel>(this.firstSavourySolutionsUser.Id);

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(
                async () => await this.SavourySolutionsUsersService.GetViewModelByIdAsync<SavourySolutionsUserEditViewModel>("3"));

            Assert.Equal(string.Format(ExceptionMessages.SavourySolutionsUserNotFound, "3"), exception.Message);
        }

        public async ValueTask DisposeAsync()
        {
            await this.connection.CloseAsync();
            await this.connection.DisposeAsync();
        }

        private void InitializeDatabaseAndRepositories()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
            var options = new DbContextOptionsBuilder<SavourySolutionsDbContext>().UseSqlite(this.connection);
            var dbContext = new SavourySolutionsDbContext(options.Options);

            dbContext.Database.EnsureCreated();

            this.SavourySolutionsUsersRepository = new EfDeletableEntityRepository<SavourySolutionsUser>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstSavourySolutionsUser = new SavourySolutionsUser
            {
                Id = "1",
                FullName = "Kiril Petrov",
                UserName = "Kiril789",
                Gender = Gender.Male,
                CreatedOn = DateTime.Parse("2020-02-10"),
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
        }

        private async Task SeedUsers()
        {
            await this.SavourySolutionsUsersRepository.AddAsync(this.firstSavourySolutionsUser);

            await this.SavourySolutionsUsersRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("SavourySolutions.Models.ViewModels"));
    }
}
