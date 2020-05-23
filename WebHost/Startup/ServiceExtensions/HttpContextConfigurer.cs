using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHost.Startup.ServiceExtensions
{
    public static class HttpContextConfigurer
    {
        public static IApplicationBuilder UseHttpContextChecker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpContextChecker>();
        }
    }

    public class HttpContextChecker
    {
        private readonly RequestDelegate _next;
        public static string localUri { get; set; }

        public HttpContextChecker(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:3000");
            localUri = httpContext.Request.Headers["Host"];
            var tester = httpContext;
            await _next(httpContext);
            
        }
    }


}
