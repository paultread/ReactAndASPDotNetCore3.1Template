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
            //auth settings submitted to front end 
            return Ok(appSettingsDTO);
        }
    }
}