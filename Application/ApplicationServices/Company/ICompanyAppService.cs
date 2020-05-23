using Application.Dtos.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationServices.Company
{
    public interface ICompanyAppService
    {
        Task<IEnumerable<CompanyDTO>> GetAllAsync();
    }
}
