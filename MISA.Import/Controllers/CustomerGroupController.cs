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
    /// <summary>
    /// CustomerGroupController
    /// </summary>
    /// CreatedBy KDLong 06/05/2021
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class CustomerGroupController : ControllerBase
    {
        DBService db = new DBService();
        /// <summary>
        /// Lấy tất cả nhóm khách hàng
        /// </summary>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
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
        /// <summary>
        /// Thêm mới một nhóm khách hàng
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
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
