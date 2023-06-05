namespace SavourySolutions.Data.Seeding;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SavourySolutions.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class SavourySolutionsDbContextSeeder : ISeeder
{
    public async Task SeedAsync(SavourySolutionsDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (dbContext == null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(SavourySolutionsDbContextSeeder));

        var seeders = new List<ISeeder>
        {
            new RolesSeeder(),
        };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(dbContext, serviceProvider);
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
        }

        if (!dbContext.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Nigerian Dish",
                    Description = "Nigerian origin dishes",
                },
                new Category
                {
                    Name = "Italian Dish",
                    Description = "Italian origin dishes",
                },
            };
            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}
