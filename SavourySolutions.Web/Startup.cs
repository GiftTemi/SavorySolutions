namespace SavourySolutions.Web
{
    using System.Reflection;

    using CloudinaryDotNet;
    using SavourySolutions.Common.Attributes;
    using SavourySolutions.Data;
    using SavourySolutions.Data.Common.Repositories;
    using SavourySolutions.Data.Models;
    using SavourySolutions.Data.Repositories;
    using SavourySolutions.Data.Seeding;
    using SavourySolutions.Models.ViewModels;
    using SavourySolutions.Services.Data;
    using SavourySolutions.Services.Data.Contracts;
    using SavourySolutions.Services.Mapping;
    using SavourySolutions.Services.Messaging;
    using SavourySolutions.Web.Hubs;
    using SavourySolutions.Web.Middlewares;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SavourySolutionsDbContext>(
                options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SavourySolutions;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddDefaultIdentity<SavourySolutionsUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<SavourySolutionsDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            services.AddScoped<PasswordExpirationCheckAttribute>();

            services.AddSignalR();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Application services
            services.AddTransient<IEmailSender>(
                serviceProvider => new SendGridEmailSender(this.configuration["SendGridSavourySolutions:ApiKey"]));
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IContactsService, ContactsService>();
            services.AddTransient<IPrivacyService, PrivacyService>();
            services.AddTransient<IFaqService, FaqService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<IRecipesService, RecipesService>();
            services.AddTransient<IArticleCommentsService, ArticleCommentsService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<ISavourySolutionsUsersService, SavourySolutionsUsersService>();
            services.AddTransient<IChatService, ChatService>();

            // External login providers
            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = "253877193959699";
                    facebookOptions.AppSecret = "573a1852a3238e2db8b005f3092e9fca";
                });

            var account = new Account(
                "dvrizvwv7",
                "131868738167915",
                "XjesLlypk_Xn51Vr0pK1-7GazRU");

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<SavourySolutionsDbContext>();
                dbContext.Database.Migrate();
                new SavourySolutionsDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithRedirects("/Home/HttpError?statusCode={0}");
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAdminMiddleware();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("subscription", "{controller=Home}/{action=ThankYouSubscription}/{email?}");
                        endpoints.MapRazorPages();
                        endpoints.MapHub<ChatHub>("/chat");
                    });
        }
    }
}
