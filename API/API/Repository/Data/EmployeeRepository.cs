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
                    Employee employee = new Employee();
                    employee.NIK = registerVM.NIK;
                    employee.FirstName = registerVM.FirstName;
                    employee.LastName = registerVM.LastName;
                    employee.PhoneNumber = registerVM.PhoneNumber;
                    employee.BirthDate = registerVM.BirthDate;
                    employee.Gender = (Models.Gender)registerVM.Gender;
                    employee.Salary = registerVM.Salary;
                    employee.Email = registerVM.Email;
                    myContext.Employees.Add(employee);

                    Account account = new Account();
                    account.NIK = registerVM.NIK;
                    account.Password = registerVM.Password;
                    myContext.Accounts.Add(account);

                    Education education = new Education();
                    education.Degree = registerVM.Degree;
                    education.GPA = registerVM.GPA;
                    education.UniversityId = registerVM.UniversityId;
                    myContext.Educations.Add(education);

                    /*Profiling profiling = new Profiling();
                    profiling.NIK = employee.NIK;
                    profiling.EducationId = education.EducationId;
                    myContext.Profilings.Add(profiling);*/

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
    }
}
