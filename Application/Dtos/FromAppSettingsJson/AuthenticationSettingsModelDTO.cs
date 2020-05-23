using Application.Models.FromAppSettingsJson.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.FromAppSettingsJson
{
    /// <summary>
    /// Sent to the client on initilisation to inform it of which Auth type it should use / is currently using between JWT and IdentityServer4
    /// Wrapped in AppSettingsModel as a prop 
    /// Only required if multiple auths are to be used
    /// </summary>
    /// <remarks/> DTO version is actually transferred (WHY??) - because db access in here? JwtBearer is not DTO though, so... find out
    public class AuthenticationSettingsModelDTO
    {
        public string Provider { get; set; }
        public JwtBearerModel JwtBearer { get; set; }
        //public IdentityServerModel IdentityServer { get; set; }

    }
}
