using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.FromAppSettingsJson;
using Application.Models.FromAppSettingsJson.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebHost.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClientAppSettingsController : BaseController //inherits from BaseController so appsettings have to be inherited
    {
        private readonly IMapper _mapper;
        private readonly IOptions<AuthenticationSettingsModel> _authSettings;

        public ClientAppSettingsController(IMapper mapper, IOptions<AuthenticationSettingsModel> authSettings, IOptions<AppSettingsModel> appSettings) : base (appSettings)
            //inherit app settings
        {
            _mapper = mapper;
            _authSettings = authSettings;
        }

        //when I used custom middleware and not teh policy underneath, it returned a '406' http status when it hit this, with the EnableCors attribute here in place
        //[EnableCors("CORSAllowLocalHost3000")]
        [Route("/ClientAppSettings")]
        [HttpGet]
        public IActionResult Get()
        {
            var appSettingsDTO = _mapper.Map<AppSettingsModelDTO>(_appSettings.Value); // Mapp AppSettings Model to DTO
            appSettingsDTO.Authentication = new AuthenticationSettingsModelDTO { Provider = _authSettings.Value.Provider }; // Add Authentication settings manually



            HttpContext.Response.Cookies.Append("TestToken1", "test1", new CookieOptions() { Secure = true, SameSite = SameSiteMode.None });

            Response.Cookies.Append("TestToken2", "test2", new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });
            Response.Cookies.Append("TestToken3", "test3", new CookieOptions() { });

            Response.Cookies.Append("TestToken4", "test4", new CookieOptions() { HttpOnly = false, Secure = false, SameSite = SameSiteMode.None });


            Response.Cookies.Append("TestToken5", "test5", new CookieOptions() { HttpOnly = false, Secure = false, SameSite = SameSiteMode.Unspecified });

            Response.Cookies.Append("TestToken6", "test6", new CookieOptions() { HttpOnly = false, Secure = false, SameSite = SameSiteMode.Lax });

            Response.Cookies.Append("TestToken7", "test7", new CookieOptions() { HttpOnly = false, Secure = false, SameSite = SameSiteMode.Strict });


            Response.Cookies.Append("TestToken8", "test8", new CookieOptions() { HttpOnly = false, Secure = false, SameSite = SameSiteMode.Strict });

            HttpContext.Response.Cookies.Append("TestToken9", "test9", new CookieOptions() { Secure = true, SameSite = SameSiteMode.None });

            //Response.Headers.Append("referer", "https://localhost:44309/");
            //Response.Headers.Append("access-control-allow-credentials", "false");
            //Response.Headers.Append("TestOne", "Testeroo");

            //auth settings submitted to front end 
            return Ok(appSettingsDTO);
        }
    }
}