namespace SavourySolutions.Web.Middlewares
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using SavourySolutions.Common;
    using SavourySolutions.Data.Models;
    using SavourySolutions.Data.Models.Enumerations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class AdminMiddleware
    {
        private readonly RequestDelegate next;

        public AdminMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<SavourySolutionsUser> userManager)
        {
            await this.SeedUserInRoles(userManager);
            await this.next(context);
        }

        private async Task SeedUserInRoles(UserManager<SavourySolutionsUser> userManager)
        {
            if (!userManager.Users.Any(x => x.UserName == GlobalConstants.AdministratorUsername))
            {
                var user = new SavourySolutionsUser
                {
                    UserName = GlobalConstants.AdministratorUsername,
                    Email = GlobalConstants.AdministratorEmail,
                    FullName = GlobalConstants.AdministratorFullName,
                    Gender = Gender.Male,
                };

                var result = await userManager.CreateAsync(user, GlobalConstants.AdministratorPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                    var newUser = await userManager.FindByEmailAsync(user.Email);
                }
                else
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
