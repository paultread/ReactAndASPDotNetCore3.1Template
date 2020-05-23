using Application.Models.FromAppSettingsJson.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.FromAppSettingsJson.Configuration
{
    /// <summary>
    /// Sent to the client on initilisation to inform it of properties it might require, e.g. which auth type is currently in use 
    /// This obj is sent to the client 
    /// </summary>
    /// <remarks/> Only required if multiple auths are to be used
    /// <remarks/> DTO version is actually transferred (WHY??) - because db access in here? JwtBearer is not DTO though, so... find out
    public class AppSettingsModel
    {
        public AuthenticationSettingsModel Authentication { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
    }
}
