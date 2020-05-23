using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebHost.Startup.ServiceExtensions
{
    public static partial class CorsConfigurer
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            //urls should not contain trailing /
            services.AddCors(options =>
            {
                options.AddPolicy("CORSAllowLocalHost3000",
                    builder =>
                    builder.WithOrigins("https://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    //.SetIsOriginAllowed(host => true)
                    .AllowCredentials()
                    );
            });


            //doesn't work
            //services.AddSession(options =>
            //{
            //    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            //});

            #region otherCorsSetups
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

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CORSAllowLocalHost3000",
            //        builder =>
            //        builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        //.WithMethods("GET", "POST")
            //        .AllowAnyHeader()
            //        //below cannot be used with the above - or at least needs to be setup
            //        ////.AllowCredentials()
            //        );
            //});


            #endregion
        }

    }

    #region doesntWork


    //https://stackoverflow.com/questions/43871637/no-access-control-allow-origin-header-is-present-on-the-requested-resource-whe

    // https://stackoverflow.com/questions/44379560/how-to-enable-cors-in-asp-net-core-webapi
    //to enable this, nothing in services.AddCors - this is not needed
    //just app.UseCorsMiddleware(), not services.UseCors(); << this is MSs middleware variant of the below
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }

    //TODO
    //1. try npm install cors --save on https://stackoverflow.com/questions/43871637/no-access-control-allow-origin-header-is-present-on-the-requested-resource-whe
    //2. https://stackoverflow.com/questions/44379560/how-to-enable-cors-in-asp-net-core-webapi - on here, try the 'checking preflight' part - nearly at the bottom with 3 votes
    //3 - above - move options status setting to the top of the request?


    public class CorsMiddleware
    {
        //this is responsible for passing the request to the next middleware in the pipeline
        //not workign with it short circuits the middleware pipeline so requests coming in to apache will not go through
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }



        public async Task InvokeAsync(HttpContext httpContext)
        {
            //http://www.playtool.com/pages/psuconnectors/connectors.html



            //httpContext.Request.Headers.Append("Sec-Fetch-Site", "cross-site");
            //httpContext.Response.Headers.Add("access-control-allow-credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:3000");
            //httpContext.Response.Headers.Add("Content-Type", "application/json; text/plain; charset=utf-8;");
            httpContext.Response.Headers.Add("Content-Type", "application/json; text/plain");
            //httpContext.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //httpContext.Response.Headers.Add("Referer", "https://localhost:3000");

            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

            //Prflight requests - https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS#Preflighted_requests
            //https://stackoverflow.com/questions/43871637/no-access-control-allow-origin-header-is-present-on-the-requested-resource-whe
            //OPTIONS type request is made for any cross-site request to see if it is safe to send it, before it sends the main request
            //...that means any crosss-site request always sends 2 requests /responses

            //REQUEST 1
            //OPTIONS is sent from client with {"Origin", "https://localhost:3000"}, {"Access-Control-Request-Method", "POST"}, {"Access-Control-Request-Headers", "X-PINGOTHER, Context-Type"}
            //200 OK is returned with all request headers, and REPSONSE HEADER {"Access-Control-Max-Age", "86400"
            //REQUEST 2
            //Main request is made, DOES NOT INCLUDE {"Access-Control.Request", "X-PINGOTHER, Context-Type"} request header
            //200 Response returned with request headers, and RESPONSE HEADERS {"Access-Control-Allow-Origin", "https://localhost:3000"}

            //A caveat to 'all cross-site requests send a preflight' is that - if no authoirzation header is present, but auth data is embedded in the POST body, no preflight
            //also - if content-type was not application/json, but instead application/x-www-form-urlencoded and a param called json that coudl be JSOn'd at the server side, no preflight would be sent
            //NEED TO TEST THE ABOVE as different for different sources
            //It is unclear as to when a preflight is sent, they are required to be known so that the logic underneath can be sorted

            if (httpContext.Request.Method == "OPTIONS")
            {
                //test the value of "Access-Control-Request-Method" - it will be e.g. POST or GET etc header to see if it contains one of the "Access-Control-Allow-Methods"

                //perform other tests using teh other headers that are there - i.e. shoudl it allow Auth headers etc found in "Access-Control-Request-Headers"

                //if so, return 200 OK (any 2xx is good)
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsync(string.Empty);

                //if not return a 501, or a 405 
            }
            //httpContext.Response.StatusCode = (int)HttpStatusCode.OK;






            //adding a response header - OnStarting modifies the response headers just before sending to the client
            httpContext.Response.OnStarting(() =>
            {
                httpContext.Response.Headers.Add("MyHeader", "GotItWorking!!!");
                return Task.FromResult(0);
            });

            //appending a reponse header if it needs it - this is more likely to be required outside of middleware

            httpContext.Response.OnStarting(() =>
            {
                int responseStatusCode = httpContext.Response.StatusCode;
                if (responseStatusCode == (int)HttpStatusCode.Created)
                {
                    IHeaderDictionary headers = httpContext.Response.Headers;
                    StringValues locationHeaderValue = string.Empty;
                    if (headers.TryGetValue("location", out locationHeaderValue))
                    {
                        httpContext.Response.Headers.Remove("location");
                        httpContext.Response.Headers.Add("location", "new location header value");
                    }
                }
                return Task.FromResult(0);
            });


            //Oncompleted fires just after the response has been sent to teh client
            httpContext.Response.OnCompleted(() =>
            {
                //httpContext.Response.Headers.Add("MyHeader", "GotItWorking!!!");
                return Task.FromResult(0);
            });



            await _next(httpContext);
        }
    }
    #endregion
}
