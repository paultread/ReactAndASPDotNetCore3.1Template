using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.Configuration
{
    /// <summary>
    /// Model for Application Settings made available to the code base from appsettings.json config
    /// </summary>
    /// <remarks>Map only the settings you wish to make available to the code base</remarks>
    public class AppSettingsModel
    {
        public AuthenticationSettingsModel Authentication { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
    }
}
