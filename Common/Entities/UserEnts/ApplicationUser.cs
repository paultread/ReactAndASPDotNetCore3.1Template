using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities.UserEnts
{
    public class ApplicationUser: IdentityUser
    {
        public string Hobbies { get; set; }
    }
}
