using MISA.Import.Entities;
using MISA.Import.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Import.Service
{
    public class DBService
    {
        private static DBRepository Instance = new DBRepository();
        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public int InsertCustomer(Customer customer)
        {
            var res = Instance.InsertCustomer(customer);
            return res;
        }
        /// <summary>
        /// Thêm mới danh sách khách hàng
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public int InsertListCustomer(IEnumerable<Customer> customers)
        {
            var res = 0;
            // Nếu Status = "" thì thêm vào db
            foreach (var customer in customers)
            {
                if (customer.Status == "")
                {
                    res += Instance.InsertCustomer(customer);
                }
            }
            return res;
        }
        /// <summary>
        /// Thêm mới nhóm khách hàng
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public int InsertCustomerGroup(CustomerGroup customerGroup)
        {
            var res = Instance.InsertCustomerGroup(customerGroup);
            return res;
        }
        /// <summary>
        /// Lấy tất cả khách hàng trên csdl
        /// </summary>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public IEnumerable<Customer> GetAllCustomers()
        {
            var res = Instance.GetAllCustomers();
            return res;
        }
        /// <summary>
        /// Lấy tất cả Customer group
        /// </summary>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public IEnumerable<CustomerGroup> GetAllCustomerGroups()
        {
            var res = Instance.GetAllCustomerGroups();
            return res;
        }
    }
}
