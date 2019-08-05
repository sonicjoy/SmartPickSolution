using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartPick.Api.DependencyInjection.Models;

namespace SmartPick.Api.DependencyInjection.Configuration
{
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection BindApplicationConfiguration(this IServiceCollection services, IConfiguration configuration, string configSectionName)
        {
            if (services == null)
                throw new ArgumentNullException(nameof (services));
            var appConfig = new ApplicationConfiguration();
            configuration.Bind(configSectionName, appConfig);
            services.AddSingleton(appConfig);
            return services;
        }
    }
}
