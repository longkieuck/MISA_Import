using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Import.Entities;
using MISA.Import.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        DBService db = new DBService();

        [HttpPost("Import")]
        public IActionResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {

            var list = new ImportService().Import(formFile, cancellationToken);
            if (list != null)
            {
                return Ok(list);
            }
            return NoContent();
        }

        [HttpPost("List-Customers")]
        public IActionResult Post([FromBody] IEnumerable<Customer> customers)
        {

            var res = db.InsertListCustomer(customers);
            if (res > 0)
            {
                return Ok(res);
            }
            return NoContent();
        }
        [HttpGet]
        public IActionResult Get()
        {

            var customers = db.GetAllCustomers();
            if (customers.Count() > 0)
            {
                return Ok(customers);
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            var res = db.InsertCustomer(customer);
            if (res > 0)
            {
                return StatusCode(201, res);
            }
            return NoContent();
        }

    }
}
