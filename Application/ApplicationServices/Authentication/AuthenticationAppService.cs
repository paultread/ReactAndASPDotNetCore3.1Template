using Application.Dtos.Identity;
using Application.Models.Authentication;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Entities.UserEnts;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Application.Models.FromAppSettingsJson.Configuration;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityServer4.Models;
using System.Linq;
using EntityFrameworkCore.DbContexts;

namespace Application.ApplicationServices.Authentication
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<AuthenticationSettingsModel> _authSettings;
        private readonly TokenValidationParameters _tokenValidationParamters;
        private readonly IdentityApplicationContext _identityDbContext;
        private readonly IMapper _mapper;
        public AuthenticationAppService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMapper iMapper, IOptions<AuthenticationSettingsModel> authAppServices, TokenValidationParameters tvparams, IdentityApplicationContext identityDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authSettings = authAppServices;
            _mapper = iMapper;
            _tokenValidationParamters = tvparams;
            _identityDbContext = identityDbContext;

        }
     

        public async Task<AuthenticationResponseModel> SignInUserAsync(AuthenticateUserModel userToAuthenticate)
        {
            var resultOfSignInAttempt = await _signInManager.PasswordSignInAsync(userToAuthenticate.Username, userToAuthenticate.Password, isPersistent: false, lockoutOnFailure: false);
            if (!resultOfSignInAttempt.Succeeded)
                return new AuthenticationResponseModel
                {
                    ClientModel = new AuthenticationResponseToClientModel
                    {
                        Errors = new[]
                        {
                            "Invalid username or password"
                        }
                    },
                    RefreshToken = ""
                };

            var authenticatedAppUser = await _userManager.FindByEmailAsync(userToAuthenticate.Username);
            var authenticatedAppUserDTO = _mapper.Map<ApplicationUserDTO>(authenticatedAppUser);


            JwtSecurityToken jwtSecurityTokenToReturn = GenerateAndReturnJwtSecurityToken(authenticatedAppUserDTO);
            SecurityToken securityTokenDerrivedFromJwtBuilderCouldReturn = GenerateAndReturnSecurityToken(authenticatedAppUserDTO);
            var jwtTokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityTokenToReturn);

            var jwtRefreshToken = GenerateAndReturnJwtToken(authenticatedAppUser, jwtSecurityTokenToReturn);
            await _identityDbContext.JwtRefreshTokens.AddAsync(jwtRefreshToken);
            await _identityDbContext.SaveChangesAsync();


            //below will only work with IDentity tokens, not JWT - so need to create own class for this, and create entity for it
            //var tester = new RefreshToken();


            return new AuthenticationResponseModel {
                ClientModel = new AuthenticationResponseToClientModel
                {
                    User = authenticatedAppUserDTO,
                    JwtAccessToken = jwtTokenString,
                    ExpiresInSeconds = _authSettings.Value.JwtBearer.ExpiryTimeInSeconds,
                    IsAuthenticated = "1",
                },
                    RefreshToken = jwtRefreshToken.RefreshTokenId
            };
        }


        private JwtRefreshToken GenerateAndReturnJwtToken(ApplicationUser user, JwtSecurityToken jwtToken)
        {
            return new JwtRefreshToken
            {
                //this is important - this is what the front end uses for the refresh token
                //this needs to be random, should not be generated using the JWT token and an algorithm as if the JWT token 
                //RefreshTokenId = Guid.NewGuid().ToString(),
                //Guid is not strong enough for this security feature, this is now generated in the Db - used an attribute tag for this
                JwtId = jwtToken.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiresAfterSeconds = 60 * 15 
            };
        }


        private JwtSecurityToken GenerateAndReturnJwtSecurityToken(ApplicationUserDTO user)
        {
            var currentDateTime = DateTime.UtcNow;

            //var tester = _authSettings.Value.JwtBearer.SecurityKey;
            return new JwtSecurityToken
           (
               issuer: _authSettings.Value.JwtBearer.Issuer,
               audience: _authSettings.Value.JwtBearer.Audience,
               claims: CreateAndReturnClaimsInArrayForUser(user),
               notBefore: currentDateTime,
               expires: currentDateTime.AddSeconds(_authSettings.Value.JwtBearer.ExpiryTimeInSeconds),
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.JwtBearer.SecurityKey)), SecurityAlgorithms.HmacSha256)
           );
        }



        private SecurityToken GenerateAndReturnSecurityToken(ApplicationUserDTO user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = CreateAndReturnClaimsInClaimsIdentityForUser(user),
                Expires = DateTime.UtcNow.AddSeconds(_authSettings.Value.JwtBearer.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.JwtBearer.SecurityKey)), SecurityAlgorithms.HmacSha256)
            };
            return new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
        }


        private Claim[] CreateAndReturnClaimsInArrayForUser(ApplicationUserDTO user)
        {
            return new Claim[] {
                new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
                new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                new Claim(type: "id", value: user.Email)
            };
        }

        private ClaimsIdentity CreateAndReturnClaimsInClaimsIdentityForUser(ApplicationUserDTO user)
        {
            return new ClaimsIdentity(
                new[] {
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type: "id", value: user.Email)
                }
            );
        }

        

       

       
        public async Task<ApplicationUserDTO> SignUpUserAsync(AuthenticateUserModel newUserToSignUp)
        {
            //return something if the user exists - would need changign to pass back an error to suggest user exists, but this means changing object returned
            var existingUser = await _userManager.FindByEmailAsync(newUserToSignUp.Username);
            if (existingUser == null)
                return new ApplicationUserDTO();

            var userToCreate = new ApplicationUser { UserName = newUserToSignUp.Username, Email = newUserToSignUp.Username, Hobbies = "Test hobby"};
            var resultOfSignUpAttempt = await _userManager.CreateAsync(userToCreate, newUserToSignUp.Password);
            if (resultOfSignUpAttempt.Succeeded)
                await _signInManager.SignInAsync(userToCreate, isPersistent: false);
            else
            {
                foreach (var error in resultOfSignUpAttempt.Errors)
                {
                    //log errors
                }
            }
            //return await _userManager.FindByEmailAsync(newUserToSignUp.Username);
            //var loggedinUser = await _userManager.FindByEmailAsync(newUserToSignUp.Username);

                return _mapper.Map<ApplicationUserDTO>( await _userManager.FindByEmailAsync(newUserToSignUp.Username));
                //loggedinUser.ProjectTo<ApplicationUserDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }



        public async Task<AuthenticationResponseModel> GenerateRefreshTokenUsingExisting(string jwtToken, string refreshToken)
        {
            //check jwtToken is of the right alg type (not specified in TokenValParams so need to do that here)
            //check it is valid (run through algorithm and check signature / date etc using validation params set)
            var potentiallyValidatedToken = GetPrincipalFromJwtToken(jwtToken);

            //if not valid return empty auth reponse with just errors in them
            if (potentiallyValidatedToken == null)
                return new AuthenticationResponseModel{ ClientModel = new AuthenticationResponseToClientModel{ Errors = new[]{"Invalid JWT" }},RefreshToken = ""};

            //this is where I am up to also - 14:11
            var expiryDateUnix = long.Parse(potentiallyValidatedToken.Claims.Single(x => String.Equals(x.Type, JwtRegisteredClaimNames.Exp)).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix)
                .Subtract(new TimeSpan(5));// sort out << 15:21

            if (expiryDateTimeUtc > DateTime.UtcNow)
                return new AuthenticationResponseModel { ClientModel = new AuthenticationResponseToClientModel { Errors = new[] { "The existing Token has not yet expired" } }, RefreshToken = "" };

            var jti = potentiallyValidatedToken.Claims.Single(x => String.Equals(x.Type, JwtRegisteredClaimNames.Jti)).Value;

            var existingRefreshTokenFromDb = _identityDbContext.JwtRefreshTokens.SingleOrDefault(rt => String.Equals(rt.RefreshTokenId, refreshToken));
            if (existingRefreshTokenFromDb == null)
                return new AuthenticationResponseModel { ClientModel = new AuthenticationResponseToClientModel { Errors = new[] { "Refresh token does not exist" } }, RefreshToken = "" };

            if (existingRefreshTokenFromDb.CreationDate.AddSeconds(existingRefreshTokenFromDb.ExpiresAfterSeconds) > DateTime.UtcNow)
                return new AuthenticationResponseModel { ClientModel = new AuthenticationResponseToClientModel { Errors = new[] { "Refresh token has expired" } }, RefreshToken = "" };
            if (existingRefreshTokenFromDb.Invalidated)
                return new AuthenticationResponseModel { ClientModel = new AuthenticationResponseToClientModel { Errors = new[] { "Refresh token is not valid for refresh anymore" } }, RefreshToken = "" };
            if (existingRefreshTokenFromDb.HasBeenUsedForARefresh)
                return new AuthenticationResponseModel { ClientModel = new AuthenticationResponseToClientModel { Errors = new[] { "Refresh token has already been used for a refresh" } }, RefreshToken = "" };
            if (existingRefreshTokenFromDb.JwtId != jti)
                return new AuthenticationResponseModel { ClientModel = new AuthenticationResponseToClientModel { Errors = new[] { "The refresh token is not valid for the given JWT JWT" } }, RefreshToken = "" };
            existingRefreshTokenFromDb.HasBeenUsedForARefresh = true;
            _identityDbContext.JwtRefreshTokens.Update(existingRefreshTokenFromDb);
            await _identityDbContext.SaveChangesAsync();

            //"id" set in the claims created
            var user = await _userManager.FindByIdAsync(potentiallyValidatedToken.Claims.Single(x => String.Equals(x.Type, "id")).Value);


            return new AuthenticationResponseModel();
        }

        private ClaimsPrincipal GetPrincipalFromJwtToken(string existingJwtToken)
        {
            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(existingJwtToken, _tokenValidationParamters, out var validatedToken);
                if (!IsJwtWithValidSecAlg(validatedToken))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecAlg(SecurityToken validatedSecurityToken)
        {
            return (validatedSecurityToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }


    }
}
