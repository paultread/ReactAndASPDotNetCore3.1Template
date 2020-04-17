using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Models.Configuration;
using AutoMapper;
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
        [Route("/ClientAppSettings")]
        [HttpGet]
        public IActionResult Get()
        {
            var appSettingsDTO = _mapper.Map<AppSettingsDTO>(_appSettings.Value); // Mapp AppSettings Model to DTO
            appSettingsDTO.Authentication = new AuthenticationSettingsDTO { Provider = _authSettings.Value.Provider }; // Add Authentication settings manually
            //auth settings submitted to front end here
            return Ok(appSettingsDTO);
        }
    }
}