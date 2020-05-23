using Application.Models.FromAppSettingsJson.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.FromAppSettingsJson.Configuration
{
    /// <summary>
    /// Sent to the client on initilisation to inform it of which Auth type it should use / is currently using between JWT and IdentityServer4
    /// Wrapped in AppSettingsModel as a prop 
    /// Only required if multiple auths are to be used
    /// </summary>
    /// <remarks/> DTO version is actually transferred (WHY??) - because db access in here? Because contains non-primatives? JwtBearer is not DTO though, so... find out
    public class AuthenticationSettingsModel
    {
        public string Provider { get; set; }
        //public IdentityServerModel IdentityServer { get; set; }
        public JwtBearerModel JwtBearer { get; set; }
    }
}
