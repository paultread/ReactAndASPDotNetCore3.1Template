using Application.Dtos.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationServices.TechInfo
{
    public class TechInfoAppService : ITechInfoAppService
    {
        private readonly IMapper _mapper;
        private readonly CrmDbContext _crmDbContext;
        public TechInfoAppService(IMapper mapper, CrmDbContext crmDbContext)
        {
            _mapper = mapper;
            _crmDbContext = crmDbContext;
        }
        public async Task<IEnumerable<TechInfoDTO>> GetAllAsync()
        {
            return await _crmDbContext.TechInfo.ProjectTo<TechInfoDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
