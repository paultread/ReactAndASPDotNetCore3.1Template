using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationServices.TechInfo
{
    public interface ITechInfoAppService
    {
        Task<IEnumerable<TechInfoDTO>> GetAllAsync();
    }
}
