using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ApplicationServices.Company;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    //need to send JWT token back to here tomorrow - in the axios interceptor
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyAppService _companyAppService;
        public CompanyController(ICompanyAppService companyAppService)
        {
            _companyAppService = companyAppService;
        }

        [Route("/Companies")]
        [HttpGet]
        //any authoirze schemes you have set up can access this
        //[Authorize]
        //this means in startup you have named the authorize scheme the default name - Bearer'
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //this means in startup you have named the authorize scheme as 'JwtBearer'
        //[Authorize(AuthenticationSchemes = "JwtBearer")]
        //Allow anonymous top trumps the authoirze tag - beware of this
        //[AllowAnonymous]
        public async Task<IActionResult> GetCompanies()
        {
            var tester = HttpContext.Request;
            return Ok(await _companyAppService.GetAllAsync());
        }
        //[Route("/Companiess")]
        [HttpGet("/Companiess")]
        //[Authorize]
        //[AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = "JwtBearer")]
        public async Task<IActionResult> GetCompaniess()
        {
            var tester = HttpContext.Request;
            return Ok(await _companyAppService.GetAllAsync());
        }


        //[HttpPost]
        //public async Task<IActionResult> GetCompaniesss(object yes)
        //{
        //    var tester = HttpContext.Request;
        //    return Ok(await _companyAppService.GetAllAsync());
        //}
    }
}