using Application.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Models.Authentication
{
    public class AuthenticationResponseModel
    {
        //stuff that gets sent to the client app to be used in the UI / front end
        public AuthenticationResponseToClientModel ClientModel { get; set; }
        //resides in a same site cookie, never to go to the client app... just the client
        public string RefreshToken { get; set; }


        //public ApplicationUserDTO User { get; set; }
        //public string IsAuthenticated { get; set; }
        ////use JwtSecurityTokenHandler.WriteToken(jwtSecurityTokenInstance) and it will give the encoded string of the three parts to this
        //public string JwtAccessToken { get; set; }
        //public string RefreshToken { get; set; }
        //public int ExpiresInSeconds { get; set; }
        //public string[] Errors { get; set; }
        ////no errors as either successful auth or none successful only
        ////can test if account is locked out but that needs a single prop
        ////public IEnumerable<string> Errors { get; set; }
    }
}
