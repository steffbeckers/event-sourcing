using CRM.Application.Common.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Files;
using CRM.Infrastructure.Identity;
using CRM.Infrastructure.Persistence;
using CRM.Infrastructure.Persistence.EventStore;
using CRM.Infrastructure.Services;
using EventStore.ClientAPI;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options => {
                    options.Clients.Add(new Client()
                    {
                        ClientId = "generic",
                        ClientName = "Generic",
                        RequireClientSecret = false,
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials
                    });
                });

            services.AddAuthentication()
                .AddIdentityServerJwt();

            IEventStoreConnection eventStoreConnection = EventStoreConnection.Create(
                connectionString: configuration.GetSection("EventStore").GetValue<string>("ConnectionString"),
                builder: ConnectionSettings.Create()
                    .KeepReconnecting()
                    .EnableVerboseLogging()
                    .UseConsoleLogger()
                    .DisableTls(), // TODO: https://github.com/EventStore/EventStore/issues/2547
                connectionName: configuration.GetSection("EventStore").GetValue<string>("ConnectionName")
            );
            eventStoreConnection.ConnectAsync().GetAwaiter().GetResult();
            services.AddSingleton(eventStoreConnection);
            services.AddSingleton<IEventsRepository<Account, Guid>, EventsRepository<Account, Guid>>();
            services.AddSingleton<IEventsService<Account, Guid>, EventsService<Account, Guid>>();
            services.AddSingleton<IEventDeserializer, EventDeserializer>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<EmailSenderAuthOptions>(configuration);

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            return services;
        }
    }
}
