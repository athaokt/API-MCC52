using API.Context;
using API.Models;
using API.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public int Delete(int nik)
        {
            var find = myContext.Employees.Find(nik);
            myContext.Employees.Remove(find);
            return myContext.SaveChanges();
        }

        public IEnumerable<Employee> Get()
        {
            var employee = myContext.Employees.ToList();
            return employee;
        }

        public Employee Get(int NIK)
        {
            return myContext.Employees.Find(NIK);
        }

        public int Insert(Employee employee)
        {
            myContext.Employees.Add(employee);
            var insert = myContext.SaveChanges();
            return insert;
        }

        public int Update(Employee employee, int nik)
        {
            var employees = myContext.Employees.Find(nik);
            employees.FirstName = employees.FirstName;
            employees.LastName = employees.LastName;
            employees.Email = employees.Email;
            employees.PhoneNumber = employees.PhoneNumber;
            employees.Salary = employees.Salary;
            employees.BirthDate = employees.BirthDate;
            myContext.Employees.Update(employee);
            var update = myContext.SaveChanges();
            return update;
        }
    }
}
