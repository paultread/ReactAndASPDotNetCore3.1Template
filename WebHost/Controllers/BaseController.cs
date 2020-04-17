using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IOptions<AppSettingsModel> _appSettings;
        public BaseController(IOptions<AppSettingsModel> appSettings)
        {
            _appSettings = appSettings;
        }
    }
}