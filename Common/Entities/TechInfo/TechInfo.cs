using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Entities.TechInfo
{
    public class TechInfo : ITechInfo
    {
        [Key]
        public int id { get; set; }
        //as strings are not nullable, do not mark as 'required'
        //expected behaviour - empty strings will be put in place
        public string TechName { get; set; }
        public string InternalAddress { get; set; }
        public string ExternalAddress { get; set; }
        public string TechPassword { get; set; }
        public int CompanyId { get; set; }
    }
}
