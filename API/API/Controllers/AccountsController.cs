using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private AccountRepository accountRepository;
        public AccountsController(AccountRepository accountRepository) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginVM loginVM)
        {
            var login = accountRepository.Login(loginVM);
            
                if (login == 2)
                {
                    return Ok(new { status = HttpStatusCode.OK, result = login, message = "Login Sukses" });
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
    }
}
