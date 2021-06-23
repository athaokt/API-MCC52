using API.Context;
using API.Models;
using API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

            public int Delete(string nik)
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

            public Employee Get(string nik)
            {
                return myContext.Employees.Find(nik);
            }

            public int Insert(Employee employee)
            {
                myContext.Employees.Add(employee);
                var insert = myContext.SaveChanges();
                return insert;
            }

            public int Update(Employee employee, string nik)
            {
                var employees = myContext.Employees.Find(nik);
                employees.FirstName = employee.FirstName;
                employees.LastName = employee.LastName;
                employees.Email = employee.Email;
                employees.PhoneNumber = employee.PhoneNumber;
                employees.Salary = employee.Salary;
                employees.BirthDate = employee.BirthDate;
                myContext.Employees.Update(employees);
                myContext.Entry(employees).State = EntityState.Modified;
                var update = myContext.SaveChanges();
                return update;
            }

        }
    
}
