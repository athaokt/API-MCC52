using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private EmployeeRepository employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public ActionResult Get(int Nik)
        {
            /*var get = employeeRepository.Get();
            return Ok(get);*/
            if (Nik == 0)
            {
                return Ok(employeeRepository.Get());
            }
            else
            {
                return Ok(employeeRepository.Get(Nik));
            }
        }

        [HttpPost]
        public ActionResult Post(Employee employee)
        {
            var insert = employeeRepository.Insert(employee);
            if(insert==1)
            {
                return Ok(insert);
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpDelete]
        public ActionResult Delete(int nik)
        {
            var response = employeeRepository.Delete(nik);
            if(nik == 0)
            {
                return BadRequest("Fail to delete");
            }
            else
            {
                if (response == 1)
                {
                    return Ok("Delete Successful");
                }
                else
                {
                    return Ok("Delete failed");
                }
            }
        }

        public ActionResult Update(Employee employee, int nik)
        {
            var response = employeeRepository.Update(employee, nik);
            if(nik == 0)
            {
                return BadRequest();
            }
            else
            {
                if (response == 1)
                {
                    return Ok("Update Success");
                }
                else
                {
                    return Ok("Fail to Update");
                }
            }
        }
    }
}
