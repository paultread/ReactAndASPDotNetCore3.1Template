using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities.Company
{
    interface ICompany
    {
        public int CompId { get; set; }
        public string CompName { get; set; }
    }
}
