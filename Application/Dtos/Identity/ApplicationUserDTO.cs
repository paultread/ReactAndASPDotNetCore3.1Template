using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;


namespace Application.Dtos.Identity
{
    /// <summary>
    /// A slimmed down model of the full ApplicationUser: IdentityUser that contains the data that the front end needs / can use on authentication
    /// It is wrapped in a UserAuthenticatedModel, passed as a prop
    /// </summary>
    public class ApplicationUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public int TwoFactorEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Hobbies { get; set; }
    }
}
