using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.Authentication;
using Application.ApplicationServices.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var request = HttpContext.Request;
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

            //TOMORROW - cookie is going back to localhost:3000 - different URL, so it is not showing up (although present in headers)... what do I do?
            HttpContext.Response.Cookies.Append("JwtRefreshTokenn", resultOfSignin.RefreshToken, new CookieOptions() { Secure = true, HttpOnly = true, SameSite = SameSiteMode.None});
            
            Response.Cookies.Append("JwtRefreshTokenn2", resultOfSignin.RefreshToken, new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None});
            Response.Cookies.Append("JwtRefreshTokenn3", resultOfSignin.RefreshToken, new CookieOptions() { });
            return Ok(resultOfSignin.ClientModel);
        }

        [Route("/SignUp")]
        [HttpPost]
        //[HttpPost("/SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] AuthenticateUserModel authModelPassedIn)
        {
            var resultOfSignUp = await _authenticationService.SignUpUserAsync(authModelPassedIn);
            return Ok(resultOfSignUp);
            //return Created("uri the newly created resource can be found at on the net", resultOfSignUp);
        }

        [HttpPost("/api/refreshtoken")]
        public async Task<IActionResult> RefreshTokenAsync(string jwtTOken, string refreshToken)
        {
            await Task.Delay(3000);
            return Ok();
        }

    }
}