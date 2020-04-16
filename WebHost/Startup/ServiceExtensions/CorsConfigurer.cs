using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHost.Startup.ServiceExtensions
{
    public static partial class CorsConfigurer
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddCors(
            //    options => options.AddPolicy(
            //        "CORSAllowLocalHost3000",
            //        policyBuilder => 
            //            policyBuilder.WithOrigins(
            //            configuration.GetSection("App").GetSection("CorsOrigins")
            //                .Value
            //                .Split(",", StringSplitOptions.RemoveEmptyEntries)
            //                .ToArray()
            //            )
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials()
            //    )

            //);

            services.AddCors(options =>
            {
                options.AddPolicy("CORSAllowLocalHost3000",
                    builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    //.WithMethods("GET", "POST")
                    .AllowAnyHeader()
                    //below cannot be used with the above - or at least needs to be setup
                    ////.AllowCredentials()
                    );
            });


            ////urls should not contain trailing /
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CORSAllowLocalHost3000",
            //        builder =>
            //        builder.WithOrigins("https://localhost:3000", "http://localhost:3000")
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials()
            //        );
            //});


        }

    }
}
