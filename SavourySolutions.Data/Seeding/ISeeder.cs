namespace SavourySolutions.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(SavourySolutionsDbContext dbContext, IServiceProvider serviceProvider);
    }
}
