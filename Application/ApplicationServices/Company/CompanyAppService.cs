using Application.Dtos.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ApplicationServices.Company
{
    public class CompanyAppService : ICompanyAppService
    {
        private readonly IMapper _mapper;
        private readonly CrmDbContext _crmDbContext;
        public CompanyAppService(IMapper imapper, CrmDbContext crmDbContext)
        {
            _mapper = imapper;
            _crmDbContext = crmDbContext;
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllAsync()
        {

            return await _crmDbContext.Company.ProjectTo<CompanyDTO>(_mapper.ConfigurationProvider).ToListAsync();

            //below still works
            //explanation - because this method is marked async, the call back is created for any code after the await point - so execution passed back to where this method was called from (bypassing the chained await recievers) and the lines of code in there will carry on executing, not the next line in this method... so either the above or the below are fine, but above is tidier

            //var companies = await _crmDbContext.Company.ToListAsync();
            //return _mapper.Map<IEnumerable<CompanyDTO>>(companies);
        }
    }
}
