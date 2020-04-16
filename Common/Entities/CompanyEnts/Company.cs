using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Entities.Company
{
    public class Company : ICompany
    { 
        [Key]
        public int CompId { get; set; }
        public string CompName { get; set; }
    }
}
