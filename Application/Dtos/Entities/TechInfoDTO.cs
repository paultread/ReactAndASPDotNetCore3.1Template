using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Entities
{
    public class TechInfoDTO
    {
        public int id { get; set; }
        public string TechName { get; set; }
        public string InternalAddress { get; set; }
        public string ExternalAddress { get; set; }
        public string TechPassword { get; set; }
        public int CompanyId { get; set; }
    }
}
