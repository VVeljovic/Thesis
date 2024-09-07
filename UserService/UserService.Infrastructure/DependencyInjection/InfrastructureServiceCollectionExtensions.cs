using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Services;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloakSettings = new KeycloakSettings();
            configuration.GetSection("KeyCloak").Bind(keycloakSettings);
            services.AddSingleton(keycloakSettings);
            services.AddHttpClient<IKeycloakService, KeycloakService>(configuration =>
            {
                configuration.BaseAddress = new Uri(keycloakSettings.AuthServerUrl);
            });

            return services;
        }
    }
}
