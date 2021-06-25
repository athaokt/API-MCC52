using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        public IConfiguration configuration;
        private AccountRepository accountRepository;
        private readonly MyContext myContext;
        public AccountsController(AccountRepository accountRepository, MyContext myContext, IConfiguration configuration) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this.myContext = myContext;
            this.configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login(LoginVM loginVM)
        {
            var login = accountRepository.Login(loginVM);
            
                if (login == 2)
                {
                var cek = myContext.Employees.Where(e => (e.Email == loginVM.Email || e.NIK == loginVM.NIK)).FirstOrDefault<Employee>();
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
                else if(login == 1)
                {
                    return BadRequest(new { status = HttpStatusCode.BadRequest, result = login, message = "Password Salah" });
                }
                else
                {
                    return BadRequest(new { status = HttpStatusCode.BadRequest, result = login, message = "NIK / Password tidak sesuai yang ada di database" });
                }
            
            
        }
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public ActionResult ResetPassword(LoginVM loginVM)
        {
            var reset = accountRepository.ResetPassword(loginVM);
            if(reset > 0)
            {
                return Ok(new { status = HttpStatusCode.OK, result = loginVM, message = "Email berhasil dikirim" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.OK, result = loginVM, message = "Gagal mengirim email" });
            }
        }
        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var reset = accountRepository.ChangePassword(changePasswordVM);
            if (reset > 0)
            {
                return Ok(new { status = HttpStatusCode.OK, result = changePasswordVM, message = "Berhasil Ganti Password" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.OK, result = changePasswordVM, message = "Gagal Ganti Password" });
            }
        }
        
        [HttpGet("GetAllData")]
        public ActionResult GetAll()
        {
            var get = accountRepository.GetAll();

            if (get != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = get, message = "Success" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = get, message = "Failed" });
            }
        }
        [HttpGet("GetAllData/{nik}")]
        public ActionResult GetAll(string nik)
        {

            var get = accountRepository.GetAll(nik);

            if (get != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = get, message = "Success" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = get, message = "Failed" });
            }
        }
    }
}
