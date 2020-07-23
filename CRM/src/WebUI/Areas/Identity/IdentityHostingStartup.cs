using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CRM.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace CRM.WebUI.Areas.Identity
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