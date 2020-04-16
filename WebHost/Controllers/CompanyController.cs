using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ApplicationServices.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
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
        public async Task<IActionResult> GetCompanies()
        {
            return Ok(await _companyAppService.GetAllAsync());
        }

    }
}