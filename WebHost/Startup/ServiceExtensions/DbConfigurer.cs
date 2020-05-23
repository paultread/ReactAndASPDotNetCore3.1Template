using EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHost.Startup.ServiceExtensions
{
    //partial classes mean the same namespace can have multiple classes with the same name, allowing people to work on the same class from in multiple projects / files
    //they are made into one file at compile time
    //same constraints as a class - i.e. no props with same name etc
    //unsure why it is partial here though
    public static class DbConfigurer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"> available to all classes Dep Inj services</param>
        /// <param name="configuration">DI service injected to allow use of DB / Entitiy framework</param>
        public static void ConfigureDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //'this' keyword above is significant...
            //as this is static class and it cannot have a constructor (where DI would be injected into), instead 'this' is used to inject it into the method instead
            //'this' makes reference to the service itself in Startup.cs and this method becomes usable in Startup.cs / makes it a service registerable on the DI pipeline from the external file

            var tester = configuration["ConnectionStrings:CRMTestLocal2"];

            services.AddDbContext<CrmDbContext>(
                options =>
                {
                    //AddDbContext extension method avaialble through Entity Framework - been added to DI pipeline.
                    //this extension method became availabel as soon as reference to the EntityFrameworkCore projetc was added

                    //UseSqlServer only became available when a direct using statement for Microsoft Entity Framework Core
                    options.UseSqlServer(
                        configuration["ConnectionStrings:CRMTestLocal2"],
                        providerOptions => {
                            //providerOptions.MigrationsAssembly(assemblyName: "EntityFrameworkCore");
                            providerOptions.EnableRetryOnFailure();
                        } 
                    );
                });

            services.AddDbContext<IdentityApplicationContext>(options =>
                 options.UseSqlServer(
                     configuration["ConnectionStrings:CRMTestLocal2"],
                     providerOptions => providerOptions.MigrationsAssembly(assemblyName: "EntityFrameworkCore")));
        }

    }
}
