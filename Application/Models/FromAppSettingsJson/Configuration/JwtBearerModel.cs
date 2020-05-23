using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.FromAppSettingsJson.Configuration
{
    /// <summary>
    /// Model used to get the JwtBearer data from appsettings.json
    /// It is wrapped in AuthenticationSettingsModel (as a prop) > wrapped in AppSettingsModel
    /// It is accessed by injecting e.g. IOptions<AuthenticationSettingsModel>  into the class in which you need access
    /// IOptions<AppSettings> is not needed if the class inherits from the BaseController a sthis is injecte din here, so _appSettings.appServices() gets injetced in the parent and accessible through the child here
    /// Used ONLY when JWT is used over IdentityServer4
    /// </summary>
    /// <remarks/> Doesn't have a DTO (why?) is it because it only contains primitives? I think it should, seeing as though we don't want to pass the SecurityKey to the UI (ever)
    public class JwtBearerModel
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryTimeInSeconds { get; set; }
    }
}
