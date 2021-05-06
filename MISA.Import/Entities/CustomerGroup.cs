using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Import.Entities
{
    /// <summary>
    /// CustomerGroup class
    /// </summary>
    /// CreatedBy KDLong 06/05/2021
    public class CustomerGroup
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid CustomerGroupId { get; set; }
        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        public string CustomerGroupName { get; set; }
    }
}
