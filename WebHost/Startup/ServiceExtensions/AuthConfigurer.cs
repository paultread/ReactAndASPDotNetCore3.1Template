using Common.Entities.UserEnts;
using EntityFrameworkCore.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHost.Startup.ServiceExtensions
{
    public static class AuthConfigurer
    {
       



            public static void ConfigureIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //without adding this in here when using JWT - the API endpoint in the Auth controller will not be hit, returns 500 error, this is likely because it cannot get a hold of the added Db Context
            //this adds Application user to the DI pipeline
            services
                .AddDefaultIdentity<ApplicationUser>
                    (options =>
                        {
                            options.SignIn.RequireConfirmedAccount = false;
                            options.Password.RequiredLength = 6;
                        }
                    )
                .AddEntityFrameworkStores<IdentityApplicationContext>();

            //below not needed for JWT - only Identity - for authorization -  I think this is to check SessionCookie ID against user to autho each APi request
            //services
            //    .AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, IdentityApplicationContext>();
        }




        public static void ConfigureJwtBearerAuthorisation(this IServiceCollection services, IConfiguration configuration)
        {

            var tokenValParams = new TokenValidationParameters
            {
                //validates the token using the secret key (third part of jwt token)
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(configuration.GetSection("Authentication").GetSection("JwtBearer").GetSection("SecurityKey").Value)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                //test clock skew - some say old JWT token works after expired & refresh
                ClockSkew = TimeSpan.Zero
            };



            //https://youtu.be/M6AkbBaDGJE tutorial 9 > 3m25s - JWT support introduced and explained


            //is AddAUtheneeded when using Authe via Identity?
            services
                .AddAuthentication(opts =>
                {
                    //opts are only added when JWT is only used, not Identity
                    //opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    //opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultAuthenticateScheme = "JwtBearer";
                    opts.DefaultScheme = "JwtBearer";
                    opts.DefaultChallengeScheme = "JwtBearer";

                })
                //whole extension method added post working
                //tells the system the 'shape' of the token - satisfies the 'Authorize' tag elements when it is passed in
                .AddJwtBearer("JwtBearer", opts =>
                {
                    opts.SaveToken = true;
                    //this is added to validate each API request that comes into the controller
                    opts.TokenValidationParameters = tokenValParams;
                })
                ;
            //.AddIdentityServerJwt();

            //add the token validation paramters into the DI container so can inject these into other classes - refresh token needs these
            services.AddSingleton(tokenValParams);
        }





        public static void ConfigureAuthLeesWay(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
               .AddJwtBearer("JwtBearer", options =>
               {
                   options.Audience = configuration["Authentication:JwtBearer:Audience"];
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       // The signing key must match!
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                       // Validate the JWT Issuer (iss) claim
                       ValidateIssuer = true,
                       ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                       // Validate the JWT Audience (aud) claim
                       ValidateAudience = true,
                       ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                       // Validate the token expiry
                       ValidateLifetime = true,

                       // If you want to allow a certain amount of clock drift, set that here
                       ClockSkew = TimeSpan.Zero
                   };
               });

        }


    }
}
