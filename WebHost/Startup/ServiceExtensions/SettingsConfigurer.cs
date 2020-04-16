using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.Configuration;

namespace WebHost.Startup.ServiceExtensions
{
    public static partial class SettingsConfigurer
    {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // Register IOptions configuration for AppConfig section of appsettings.json
            services.Configure<AppSettingsModel>(configuration.GetSection("App"));
            services.Configure<AuthenticationSettingsModel>(configuration.GetSection("Authentication"));
        }

    }
}
