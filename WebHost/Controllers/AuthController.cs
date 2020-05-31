using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.Authentication;
using Application.ApplicationServices.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationAppService _authenticationService;

        public AuthController(IAuthenticationAppService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        //[Route("/Refresh"]
        //[HttpPost]
        //public async Task<IActionResult> RefreshJwtToken([FromBody] JwtRefreshToken refreshToken, string jwtToken )
        //{
        //    return await Ok();
        //}

        [Route("/SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignInAsync([FromBody] AuthenticateUserModel authentModelPassedIn) 
            //public async Task<IActionResult> SignInAsync([FromBody] object authentModelPassedIn)
            //public async Task<IActionResult> SignInAsync()
        {
            var resultOfSignin = await _authenticationService.SignInUserAsync(authentModelPassedIn);
            //var person = authentModelPassedIn;
            //if post request is made with: 
            //post('url', 'string to send') - 
            //below can get value pair as string (it comes in the form obj)
            //var tt = HttpContext.Request.Form.FirstOrDefault();

            //as soon as you wrap the string in an object - like:
            //post('url', {'string to send'})
            //form object is taken out
            //Can decorate with [FromForm] and [FromBody] tags, but the object you use must be one tha that the JSON object can be parsed to
            if (String.IsNullOrWhiteSpace(resultOfSignin.ClientModel.IsAuthenticated))
                    //THIS IS WRONG - returns 400 not a 401
                //return BadRequest(resultOfSignin); 
                    //returns errors in empty auth response
                //return Ok(resultOfSignin);
                return Unauthorized(resultOfSignin.ClientModel);


            //create refresh token
            // add to db
            // return string

            //add to cookie in here

            HttpContext.Response.Cookies.Append("JwtRefreshToken", resultOfSignin.RefreshToken, new CookieOptions() { Secure = true, HttpOnly = true, SameSite = SameSiteMode.None });

            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");

            return Ok(resultOfSignin.ClientModel);
        }

        [Route("/SignUp")]
        [HttpPost]
        //[HttpPost("/SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] AuthenticateUserModel authModelPassedIn)
        {
            var resultOfSignUp = await _authenticationService.SignUpUserAsync(authModelPassedIn);
            return Ok(resultOfSignUp);
            //below is best way to do it
            //return Created("uri the newly created resource can be found at on the net", resultOfSignUp);
        }
        //currently the silent refresh is handled at the client - can intercept using implementation from the website below and silently refresh at server side - but might lead to issues in the client (JWT token would need swapping out and this would need managing when it is returned)
        //https://channel9.msdn.com/Series/aspnetmonsters/ASPNET-Monsters-111-Authorize-Tag-Helper

        [HttpPost("/api/refreshtoken")]
        public async Task<IActionResult> RefreshTokenAsync(string jwtToken)
        {
            //refresh token from http only context - that way never enters the app so not prone to csrf
            var jwtRefreshToken = HttpContext.Request.Cookies["JwtRefreshToken"].Trim();
            //HERE WHERE COME BACK TO THIS
            //Now - bool for 'keep logged in' - filter into the equation and update the JWT / RT flow
            //JWT == expired && RT == valid = new JWT
            
            //JWT == expired && RT == expired = && KLI ?? (relogin?)
            //JWT == valid && RT == expired && KLI = ??
            //JWT == invalid && RT = expired && KLI ??
            //JWT == null && RT = expired && KLI = ??








            var tester = _authenticationService.GenerateRefreshTokenUsingExisting(jwtToken, jwtRefreshToken);
            if (tester.Result.ClientModel.Errors != null)
            {
                //
                //
            }
            return Ok();
        }

    }
}