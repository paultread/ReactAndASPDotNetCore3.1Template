using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public class AppSettingsDTO
    {
        public AuthenticationSettingsDTO Authentication { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
    }
}
