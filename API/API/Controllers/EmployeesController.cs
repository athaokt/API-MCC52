using API.Base;
using API.Context;
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
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVm)
        {
            var insert = employeeRepository.Register(registerVm);
            if (insert == 2)
            {
                return Ok(new { status = HttpStatusCode.OK, result = insert, message = "Success" });
            }
            else if(insert == 1)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = insert, message = "Email tidak boleh sama" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = insert, message = "NIK tidak boleh sama" });
            }
        }
        

    }
}
