using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly MyContext myContext;

        public TokenController(IConfiguration config, MyContext context)
        {
            configuration = config;
            myContext = context;
        }

        [HttpPost]
        public IActionResult Post(LoginVM loginVM)
        {
            var cek = myContext.Employees.Where(e => (e.Email == loginVM.Email || e.NIK == loginVM.NIK)).FirstOrDefault<Employee>();
            if (cek != null)
            {
                var cekPass = BCrypt.Net.BCrypt.Verify(loginVM.Password, cek.Account.Password);
                if (cekPass)
                {
                    var email = myContext.Employees.FirstOrDefault(e => e.Email == loginVM.Email);
                    var role = myContext.AccountRoles.FirstOrDefault(a => a.NIK == email.NIK);
                    var find = myContext.Roles.FirstOrDefault(a => a.RoleId == role.RoleId);

                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("NIK", cek.NIK),
                    new Claim("FirstName", cek.FirstName),
                    new Claim("LastName", cek.LastName),
                    new Claim("Email", cek.Email),
                    new Claim("role", find.RoleName)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                    var write = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { status = HttpStatusCode.OK, result = write, message = "Success" });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        
    }
}
