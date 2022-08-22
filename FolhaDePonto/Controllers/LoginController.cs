using Application.Authorization;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityManager _identityManager;

        public UserController(UserManager<ApplicationUser> userManager,
            IIdentityManager identityInitializer)
        {
            _userManager = userManager;
            _identityManager = identityInitializer;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public object Post(
            [FromBody] User user,
            [FromServices] SignInManager<ApplicationUser> signInManager,
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfigurations tokenConfigurations)
        {

            bool validCredentials = false;
            if (user != null && !String.IsNullOrWhiteSpace(user.UserID))
            {
                var userIdentity = _userManager
                    .FindByNameAsync(user.UserID).Result;
                if (userIdentity != null)
                {
                    var resultadoLogin = signInManager
                        .CheckPasswordSignInAsync(userIdentity, user.Password, false)
                        .Result;
                    if (resultadoLogin.Succeeded)
                    {
                        validCredentials = _userManager.IsInRoleAsync(
                            userIdentity, Roles.ROLE_CLIENT).Result;
                    }
                }
            }

            if (validCredentials)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.UserID, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserID)
                    }
                );

                DateTime creationDate = DateTime.Now;
                DateTime expiringDate = creationDate +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = creationDate,
                    Expires = expiringDate
                });
                var token = handler.WriteToken(securityToken);

                return new JWTResponse
                {
                    Authenticated = true,
                    Created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = expiringDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    AccessToken = token,
                    Message = "OK"
                };
            }
            else
            {
                return new JWTResponse
                {
                    Authenticated = false,
                    Message = "Auth failed"
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public ObjectResult CreateUserPost(
            [FromBody] User usuario)
        {
            _identityManager.CreateUser(usuario);
            return new OkObjectResult(true);
        }
    }
}
