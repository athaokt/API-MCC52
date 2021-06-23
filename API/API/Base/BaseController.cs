using API.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }
        public ActionResult Get()
        {
            var get = repository.Get();

            if (get != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = get, message = "Success" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = get, message = "Failed" });
            }
        }
        [HttpGet("{key}")]
        public ActionResult Get(Key key)
        {
            var getById = repository.Get(key);
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
        public ActionResult Post(Entity entities)
        {
            var insert = repository.Insert(entities);
            if (insert == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = insert, message = "Success" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = insert, message = "Failed" });
            }

        }

        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            var delete = repository.Delete(key);
            if (key == null)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = delete, message = "Delete Failed" });
            }
            else
            {

                return Ok(new { status = HttpStatusCode.OK, result = delete, message = "Delete Success" });
            }
        }
        [HttpPut("{key}")]
        public ActionResult Put(Entity entities, Key key)
        {   
            var update = repository.Update(entities, key);
            if (update != 0)
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
