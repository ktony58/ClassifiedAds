using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ClassifiedAds.Areas.Identity.IdentityHostingStartup))]
namespace ClassifiedAds.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}