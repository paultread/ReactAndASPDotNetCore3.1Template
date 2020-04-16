using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebHost.Startup.ServiceExtensions
{
    public static class DIContainersConfigurer
    { 
        public static void RegisterDIContainers(this IServiceCollection services)
        {
            // ApplicationServices DI's - Register all Interface Types in ApplicationServices namespace that is an AppService
            var appServiceRegistrations =
                from type in Assembly.Load("Application").GetTypes()
                where !type.IsInterface && !type.IsAbstract && !type.IsEnum && type.Name.ToUpper().Contains("APPSERVICE") && type.Namespace.Contains("ApplicationServices")
                select new { Interface = type.GetInterfaces().Single(), Implementation = type };
            foreach (var reg in appServiceRegistrations)
            {
                //adds each app service as a Transient service
                services.AddTransient(reg.Interface, reg.Implementation);
            }
        }

    }
}
