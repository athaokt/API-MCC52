using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();
        Employee Get(int NIK);
        int Insert(Employee employee);
        int Update(Employee employee, int nik);
        int Delete(int nik);
    }
}
