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
        public int InsertCustomer(Customer customer)
        {
            var res = Instance.InsertCustomer(customer);
            return res;
        }
        public int InsertListCustomer(IEnumerable<Customer> customers)
        {
            var res = 0;
            foreach (var customer in customers)
            {
                if (customer.Status == "")
                {
                    res += Instance.InsertCustomer(customer);
                }
            }
            return res;
        }
        public int InsertCustomerGroup(CustomerGroup customerGroup)
        {
            var res = Instance.InsertCustomerGroup(customerGroup);
            return res;
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            var res = Instance.GetAllCustomers();
            return res;
        }
        public IEnumerable<CustomerGroup> GetAllCustomerGroups()
        {
            var res = Instance.GetAllCustomerGroups();
            return res;
        }
    }
}
