using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Models.Authentication
{
    /// <summary>
    /// Used to parse and hold the username and password passed in to authenticate API to then be used with Identity Server / auth process to e.g. sign them in & out 
    /// </summary>
    public class AuthenticateUserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public Boolean KeepLoggedIn { get; set; }
    }
}
