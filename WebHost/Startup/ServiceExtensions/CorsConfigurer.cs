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
                    .AllowCredentials() // sets "Access-Control-Allow-Credentials" to true << required for setting cookies on the client (alongside import axios... axios.defaults.withCredentials = true) 
                    );
            });
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
            //        //.SetIsOriginAllowed(host => true)
            //        .AllowAnyHeader()
            //        //below cannot be used with the above - or at least needs to be setup
            //        ////.AllowCredentials()
            //        );
            //});


            #endregion
        }

    }

    // to enable this method, just app.UseCorsMiddleware(), not services.UseCors(); << this is MSs middleware variant of the below
    //to enable Microsofts middleware, use both services.addCors() and app.enableCors("name of policy") << this is in place and workign now
    #region setCorsProgramaticallyYourself
    //@@@@@@@@@@@@@@@@@@@@@ learning & resources
    //https://stackoverflow.com/questions/43871637/no-access-control-allow-origin-header-is-present-on-the-requested-resource-whe
    // https://stackoverflow.com/questions/44379560/how-to-enable-cors-in-asp-net-core-webapi

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
    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }

    public class CorsMiddleware
    {
        //this is responsible for passing the request to the next middleware in the pipeline
        //not working with it short circuits the middleware pipeline so requests coming in to apache will not go through
        private readonly RequestDelegate _next;
        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            //required for setting cookies on the client (alongside import axios... axios.defaults.withCredentials = true)
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //rest are self-detailing
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:3000");
            httpContext.Response.Headers.Add("Content-Type", "application/json; text/plain");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            #region comments
                //Others that you might set - but not needed for functionality 
                //httpContext.Request.Headers.Append("Sec-Fetch-Site", "cross-site");
                //httpContext.Response.Headers.Add("Referer", "https://localhost:3000");
            #endregion

            if (httpContext.Request.Method == "OPTIONS")
            {
                #region commentsDetailignRequiredFunctionalityOPENME
                    //test the value of "Access-Control-Request-Method" - it will be e.g. POST or GET etc header to see if it contains one of the "Access-Control-Allow-Methods"
                    //perform other tests using the other headers that are there - i.e. should it allow 'Authorization' headers etc found in "Access-Control-Request-Headers"
                    //if so, return 200 OK (any 2xx is good)
                #endregion
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsync(string.Empty);
                //if the preflight requirements are not met, return a 501 or a 405 - done elsewhere << auto by Dot Net Core... and never accept the main request after the preflight
            }
            #region onStartingAndOnCompletedHttpContextMethods
                //adding a response header - OnStarting modifies the response headers just before sending to the client
                //httpContext.Response.OnStarting(() =>
                //{
                //    httpContext.Response.Headers.Add("MyHeader", "GotItWorking!!!");
                //    return Task.FromResult(0);
                //});

                //appending a reponse header if it needs it - this is more likely to be required outside of middleware

                //httpContext.Response.OnStarting(() =>
                //{
                //    int responseStatusCode = httpContext.Response.StatusCode;
                //    if (responseStatusCode == (int)HttpStatusCode.Created)
                //    {
                //        IHeaderDictionary headers = httpContext.Response.Headers;
                //        StringValues locationHeaderValue = string.Empty;
                //        if (headers.TryGetValue("location", out locationHeaderValue))
                //        {
                //            httpContext.Response.Headers.Remove("location");
                //            httpContext.Response.Headers.Add("location", "new location header value");
                //        }
                //    }
                //    return Task.FromResult(0);
                //});


                //Oncompleted fires just after the response has been sent to the client
                //httpContext.Response.OnCompleted(() =>
                //{
                //    //httpContext.Response.Headers.Add("MyHeader", "GotItWorking!!!");
                //    return Task.FromResult(0);
                //});
            #endregion
            await _next(httpContext);
        }
    }
    #endregion
}
