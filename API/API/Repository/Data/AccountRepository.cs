using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int Login(LoginVM loginVM)
        {
            var cek = myContext.Employees.Where(e => (e.NIK == loginVM.NIK) || (e.Email == loginVM.Email)).FirstOrDefault<Employee>();
            if (cek != null)
            {
                var cekPass = BCrypt.Net.BCrypt.Verify(loginVM.Password, cek.Account.Password);
                if (cekPass)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }

        public int ResetPassword(LoginVM loginVM)
        {
            Guid g = Guid.NewGuid();
            var getEmail = loginVM.Email;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("oktigalasatha@gmail.com");
            mail.To.Add(getEmail);
            mail.Subject = "Test Mail";
            mail.Body = g.ToString();

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("oktigalasatha@gmail.com", "Atha.1998");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            var find = myContext.Employees.Where(e => e.Email == getEmail).FirstOrDefault<Employee>();
            var find2 = myContext.Accounts.Find(find.NIK);

            find2.Password = BCrypt.Net.BCrypt.HashPassword(g.ToString(), GetRandomSalt());

            myContext.SaveChanges();

            return 1;
        }
        public int ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var cek = myContext.Employees.Where(e => (e.NIK == changePasswordVM.NIK) || (e.Email == changePasswordVM.Email)).FirstOrDefault<Employee>();
            if (cek != null)
            {
                var cekPass = BCrypt.Net.BCrypt.Verify(changePasswordVM.OldPassword, cek.Account.Password);
                if (cekPass)
                {
                    var change = myContext.Accounts.Find(cek.NIK);
                    change.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordVM.NewPassword, GetRandomSalt());
                    myContext.SaveChanges();
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
        public static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }
    }
}