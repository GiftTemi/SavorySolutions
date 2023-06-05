using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SavourySolutions.Web.Areas.Identity.IdentityHostingStartup))]

namespace SavourySolutions.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
