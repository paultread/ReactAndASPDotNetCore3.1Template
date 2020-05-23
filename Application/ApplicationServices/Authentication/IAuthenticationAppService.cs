using Application.Dtos.Identity;
using Application.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Application.ApplicationServices.Authentication
{
    public interface IAuthenticationAppService
    {
        public Task<AuthenticationResponseModel> SignInUserAsync(AuthenticateUserModel userToAuthenticate);
        public Task<ApplicationUserDTO> SignUpUserAsync(AuthenticateUserModel newUserToSignUp);

        public Task<AuthenticationResponseModel> GenerateRefreshTokenUsingExisting(string jwtToken, string refreshToken);

    }
}
