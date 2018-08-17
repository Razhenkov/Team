using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Team.Services;
using Team.Services.Domains;
using Team.Web.Models;

namespace Team.Web.Controllers
{
    [Authorize(Policy = "RequireUserRole")]
    public class UserController : Controller
    {
        private readonly IUserServices userServices;
        private readonly JwtConfig jwtConfig;

        public UserController(IUserServices userServices, IOptions<JwtConfig> jwtConfig)
        {
            this.userServices = userServices;
            this.jwtConfig = jwtConfig.Value;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromQuery(Name = "e")]string emailAddress,
            [FromQuery(Name = "p")]string password)
        {
            if (ModelState.IsValid)
            {
                var result = await userServices.Login(emailAddress, password);
                if (result != UserLoginResult.Success)
                    return Ok(result.ToString());

                var token = GenerateAuthenticationToken(emailAddress);

                return Ok(new UserLoginResponse { Token = token });
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await userServices.Add(model.FirstName.Trim(), model.LastName.Trim(), model.EmailAddress.Trim(), model.Password.Trim());

                return Ok();
            }

            return BadRequest();
        }

        public async Task<List<User>> List()
        {
            return await userServices.List();
        }

        [NonAction]
        private string GenerateAuthenticationToken(string emailAddress)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, emailAddress),
                new Claim(ClaimTypes.Authentication, "true"),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    issuer: jwtConfig.Issuer,
                    audience: jwtConfig.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(90),
                    signingCredentials: creds)
               );

            return token;
        }
    }
}
