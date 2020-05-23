using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using CompanyModel = Common.Entities.Company.Company;
using Application.Dtos.Entities;
using Application.Dtos.FromAppSettingsJson;
using Application.Dtos.Identity;
using TechInfoModel = Common.Entities.TechInfo.TechInfo;
using AppSettingsModel = Application.Models.FromAppSettingsJson.Configuration.AppSettingsModel;
using Common.Entities.UserEnts;

namespace Application.ApplicationServices.Mappings
{
    /// <summary>
    /// Automapper profile that is required to map objects and is registered in the application pipeline
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // IMPORTANT: Only need to create mappings for objects where the properties are not an exact match
            //Above doesn't seem to be straightforwardly true..., as the below mapping is required
            CreateMap<CompanyModel, CompanyDTO>();
            CreateMap<TechInfoModel, TechInfoDTO>();
            CreateMap<AppSettingsModel, AppSettingsModelDTO>();
            CreateMap<ApplicationUser, ApplicationUserDTO>();
        }
    }
}
