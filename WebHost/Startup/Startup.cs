using Application.ApplicationServices.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WebHost.Startup.ServiceExtensions;

namespace WebHost.Startup
{

    public class Startup
    {
        public IConfiguration Configuration { get; }

        //had to remove original startup constructor when using API only debug env, as secrets were not being added
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}



        //http://danpatrascu.com/middleware-in-asp-net-core-part-i/
        //above good website on order of middleware
        //this is run through for each request (e.g. http, forwards for the request, backwards for the response)
        //these methods aren't actually hit at all until the first request from the client comes in
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

     

        // This method gets called at runtime. Use this method to add services to the container.
        //MIDDLEWARE order in here does NOT matter
        public void ConfigureServices(IServiceCollection services)
        {
            //needed to access HttpContext.requests... etc, not figured out where I need this yet - was usign to detect the requesting URI in here so that in Configure - on each Http request, if a specific URI then teh front end woudl also be served rather than just the API
            //services.AddHttpContextAccessor();

            //on startup > configure CORS to use for the remainder of the session
            services.ConfigureCors(Configuration);
            services.AddControllersWithViews();
            
            // In production, the React files will be served from this directory - this is the one to host
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



            //used to setup AspUsers functionality. This extends as far as e.g. session authorization, but this is ignored in favour of JWT...
            services.ConfigureIdentityAuthentication(Configuration);
            //used to setup JWT and JWT Refresh Tokens to maintain authentication
            services.ConfigureJwtBearerAuthorisation(Configuration);

            //services.ConfigureAuthLeesWay(Configuration);


            //add e.g. CompanyAppService
            services.RegisterDIContainers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //MIDDLEWARE order in here DOES matter
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

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

            //look into, if HttpContext.Request.Headers["Host"] === "https://localhost:44309/" << to replace && 4 == 3 -- this should get the front end workign for the secure URL rather than that runnign in localhost:3000
            if (env.IsEnvironment("APIOnlyDevelopment") && 4 == 3)
                app.UseSpaStaticFiles();



            app.UseRouting();
            //this adds application middleware...but...
            //why is it needed? It works without? I am guessing it has something to do with the API checking the token?

            //these are both required to use the 'authorize' tag on controllers and methods
            //without both of these the response is a 500 server error
            //Auth(s) needs to also be placed here between useRouting and UseEnpoints or gives an error

            app.UseAuthentication();
            app.UseAuthorization();


            //this has to be between specific middleware (useRouting and UseEndPoints) also
            app.UseCors("CORSAllowLocalHost3000"); // Enable CORS!
            //different way to do things
            //app.UseCors(opts => opts.WithOrigins("https://localhost:3000").AllowAnyMethod());


            //doing it programatically yourself below (doesn't work)
            //app.UseCorsMiddleware();


            //look into - uncomment to bring in custom middleware to grab HttpContext (although need to read more on best practice and it is not working yet)
            //app.UseHttpContextChecker();

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
                    spa.UseReactDevelopmentServer(npmScript: "start");
                //spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");

                //look into, if HttpContext.Request.Headers["Host"] === "https://localhost:44309/" << to replace && 4 == 3 -- this should get the front end working for the secure URL rather than that running in localhost:3000
                if (Equals(env.EnvironmentName, "APIOnlyDevelopment") && 4 == 3)
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
            });
        }
    }
}
