using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        public IConfiguration _configuration;
        private readonly MyContext myContext;
        public AccountRepository(IConfiguration config, MyContext myContext) : base(myContext)
        {
            _configuration = config;
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
            var cek = myContext.Employees.Where(e => (e.NIK == loginVM.NIK) || (e.Email == loginVM.Email)).FirstOrDefault<Employee>();
            if (cek != null)
            {
                Guid g = Guid.NewGuid();
                var getEmail = loginVM.Email;

                DateTime dateTime = DateTime.Now;
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("oktigalasatha@gmail.com");
                mail.To.Add(getEmail);
                mail.Subject = $"Password Sementara {dateTime}";
                mail.Body = $"Berikut adalah password sementara untuk melakukan reset password:\n{g.ToString()}";

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
            else
            {
                return 0;
            }
            
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
        
        public IQueryable GetAll()
        {

            var employeeRecord = (from e in myContext.Employees
                                  join a in myContext.Accounts on e.NIK equals a.NIK
                                  join ar in myContext.AccountRoles on a.NIK equals ar.NIK
                                  join r in myContext.Roles on ar.RoleId equals r.RoleId
                                  join p in myContext.Profilings on a.NIK equals p.NIK
                                  join ed in myContext.Educations on p.EducationId equals ed.EducationId
                                  join u in myContext.Universities on ed.UniversityId equals u.UniversityId
                                  select new 
                                  {
                                      e.NIK,
                                      e.FirstName,
                                      e.LastName,
                                      e.Email,
                                      e.Salary,
                                      e.PhoneNumber,
                                      e.Gender,
                                      e.BirthDate,
                                      ed.Degree,
                                      ed.GPA,
                                      u.Name,
                                      r.RoleName
                                  });

            return employeeRecord;
        }

        public IQueryable GetAll(string nik)
        {
            

            var employeeRecord = (from e in myContext.Employees 
                                  join a in myContext.Accounts on e.NIK equals a.NIK
                                  join ar in myContext.AccountRoles on a.NIK equals ar.NIK
                                  join r in myContext.Roles on ar.RoleId equals r.RoleId
                                  join p in myContext.Profilings on a.NIK equals p.NIK
                                  join ed in myContext.Educations on p.EducationId equals ed.EducationId
                                  join u in myContext.Universities on ed.UniversityId equals u.UniversityId
                                  where e.NIK == $"{nik}"
                                  select new
                                  {
                                      e.NIK,
                                      e.FirstName,
                                      e.LastName,
                                      e.Email,
                                      e.Salary,
                                      e.PhoneNumber,
                                      e.Gender,
                                      e.BirthDate,
                                      ed.Degree,
                                      ed.GPA,
                                      u.Name,
                                      r.RoleName
                                  });

            return employeeRecord;
        }

        public static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }
    }
}