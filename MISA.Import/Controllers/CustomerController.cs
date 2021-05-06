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
    /// <summary>
    /// CustomerController
    /// </summary>
    /// CreatedBy KDLong 06/05/2021
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        DBService db = new DBService();
        /// <summary>
        /// Api thực hiện chọn 1 file nhập liệu và kiểm tra các dữ liệu trong file
        /// </summary>
        /// <param name="formFile">tệp nhập khẩu</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// List các khách hàng và trạng thái thỏa mãn hay không
        /// </returns>
        /// CreatedBy KDLong 06/05/2021
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
        /// <summary>
        /// Api thực hiện thêm list các khách hàng
        /// </summary>
        /// <param name="customers">Danh sách khách hàng</param>
        /// <returns>
        /// Số khách hàng thỏa mãn và thêm được
        /// </returns>
        /// CreatedBy KDLong 06/05/2021
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
        /// <summary>
        /// Lấy tất cả khách hàng trong database
        /// </summary>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
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
        /// <summary>
        /// Thêm mới 1 khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
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
