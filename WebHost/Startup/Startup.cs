using Application.ApplicationServices.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebHost.Startup.ServiceExtensions;

namespace WebHost.Startup
{

    public class Startup
    {
        public IConfiguration Configuration { get; }

        //had to remove original startup constructor when using API only debug env, as secrets were nto being added
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        //test

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
            //.AddJsonFile("appsettings.Development.jsonjson",
            //        optional: false,
            //        reloadOnChange: true)
            .AddEnvironmentVariables()
            //can't pass args into here from previous class
            //.AddCommandLine()
            ;

            if (env.IsDevelopment() || Equals(env.EnvironmentName, "APIOnlyDevelopment"))
            {
                builder.AddUserSecrets<Startup>();
                builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            }

            Configuration = builder.Build();
        }

     

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors(Configuration);
            services.AddControllersWithViews();
            

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            // Register AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.ConfigureSettings(Configuration);

            // Configure DbContexts for Entity Framework Core
            //has to be configured before RegisteringDiContainers
            services.ConfigureDbContexts(Configuration);
            //add e.g. CompanyAppService
            services.RegisterDIContainers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors("CORSAllowLocalHost3000"); // Enable CORS!
            //app.UseCors(); // Enable CORS!
            if (env.IsDevelopment() || env.IsEnvironment("APIOnlyDevelopment"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsEnvironment("APIOnlyDevelopment")) 
                app.UseSpaStaticFiles();
            //app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                    
                }
            });
        }
    }
}
