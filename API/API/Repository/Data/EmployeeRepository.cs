using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        
        private readonly MyContext myContext;

        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }
        public int Register(RegisterVM registerVM)
        {

            var cekNik = myContext.Employees.Find(registerVM.NIK);
            if (cekNik == null)
            {
                var cekEmail = myContext.Employees.Where(e => e.Email == registerVM.Email).FirstOrDefault<Employee>();
                if (cekEmail == null)
                {

                    /*var getRandomSalt = BCrypt.Net.BCrypt.GenerateSalt(12);*/

                    Employee employee = new Employee();
                    Account account = new Account();
                    Education education = new Education();
                    Profiling profiling = new Profiling();

                    employee.NIK = registerVM.NIK;
                    employee.FirstName = registerVM.FirstName;
                    employee.LastName = registerVM.LastName;
                    employee.PhoneNumber = registerVM.PhoneNumber;
                    employee.BirthDate = registerVM.BirthDate;
                    employee.Gender = (Models.Gender)registerVM.Gender;
                    employee.Salary = registerVM.Salary;
                    employee.Email = registerVM.Email;
                    myContext.Employees.Add(employee);
                    myContext.SaveChanges();

                    
                    account.NIK = registerVM.NIK;
                    account.Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password, GetRandomSalt());
                    myContext.Accounts.Add(account);
                    myContext.SaveChanges();

                    
                    education.Degree = registerVM.Degree;
                    education.GPA = registerVM.GPA;
                    education.UniversityId = registerVM.UniversityId;
                    myContext.Educations.Add(education);
                    myContext.SaveChanges();

                    
                    profiling.NIK = registerVM.NIK;
                    profiling.EducationId = education.EducationId;
                    myContext.Profilings.Add(profiling);

                    myContext.SaveChanges();

                    
                    return 2;
                }
                return 1;
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
