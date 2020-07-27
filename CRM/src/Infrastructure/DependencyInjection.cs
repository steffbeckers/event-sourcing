using CRM.Application.Common.Interfaces;
using CRM.Infrastructure.Files;
using CRM.Infrastructure.Identity;
using CRM.Infrastructure.Persistence;
using CRM.Infrastructure.Services;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("CRMDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            IEventStoreConnection eventStoreConnection = EventStoreConnection.Create(
                connectionString: configuration.GetSection("EventStore").GetValue<string>("ConnectionString"),
                builder: ConnectionSettings.Create().KeepReconnecting(),
                connectionName: configuration.GetSection("EventStore").GetValue<string>("ConnectionName")
            );
            eventStoreConnection.ConnectAsync().GetAwaiter().GetResult();
            services.AddSingleton(eventStoreConnection);

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<EmailSenderAuthOptions>(configuration);

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}
