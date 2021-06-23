using API.Models;
using API.Repository;
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
    public class EmployeesControllerOld : ControllerBase
    {
        private EmployeeRepository employeeRepository;

        public EmployeesControllerOld(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var get = employeeRepository.Get();

            if (get != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = get, message = "Success" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = get, message = "Failed" });
            }
        }

        [HttpGet("{nik}")]
        public ActionResult Get(string nik)
        {
            var getById = employeeRepository.Get(nik);
            if (getById != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = getById, message = "Success" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, result = getById, message = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult Post(Employee employee)
        {
            var insert = employeeRepository.Insert(employee);
            if (insert == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = insert, message = "Success" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = insert, message = "Failed" });
            }

        }

        [HttpDelete("/API/Employees/{nik}")]
        public ActionResult Delete(string nik)
        {
            var delete = employeeRepository.Delete(nik);
            if (nik == null)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = delete, message = "Delete Failed" });
            }
            else
            {

                return Ok(new { status = HttpStatusCode.OK, result = delete, message = "Delete Success" });
            }
        }
        [HttpPut("/API/Employees/{nik}")]
        public ActionResult Put(Employee employee, string nik)
        {
            var find = employeeRepository.Get(nik);
            var update = employeeRepository.Update(employee, nik);
            if (find != null)
            {

                return Ok(new { status = HttpStatusCode.OK, result = update, message = "Update Success" });

            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, result = update, message = "Update Failed" });
            }
        }
    }
}
