using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Import.Entities;
using MISA.Import.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Import.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class CustomerGroupController : ControllerBase
    {
        DBService db = new DBService();
        [HttpGet]
        public IActionResult Get()
        {

            var customerGroups = db.GetAllCustomerGroups();
            if (customerGroups.Count() > 0)
            {
                return Ok(customerGroups);
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CustomerGroup customerGroup)
        {
            var res = db.InsertCustomerGroup(customerGroup);
            if (res > 0)
            {
                return StatusCode(201, res);
            }
            return NoContent();
        }
    }
}
