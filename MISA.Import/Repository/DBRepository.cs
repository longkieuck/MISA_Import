using Dapper;
using MISA.Import.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Import.Repository
{
    public class DBRepository
    {
        protected string connectionString = "" +
           "Host = 47.241.69.179;" +
           "Port = 3306;" +
           "Database = MF822_Import_KDLong;" +
           "User Id= dev;" +
           "Password = 12345678;";
        protected IDbConnection dbConnection;
        /// <summary>
        /// Thêm mới 1 khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// CreatedBy KDLong 06/05/2021
        public int InsertCustomer(Customer customer)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var rowsAffect = dbConnection.Execute($"Proc_InsertCustomer", param: customer, commandType: CommandType.StoredProcedure);
                return rowsAffect;
            }
        }
        /// <summary>
        /// Thêm mới nhóm khách hàng
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public int InsertCustomerGroup(CustomerGroup customerGroup)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var rowsAffect = dbConnection.Execute($"Proc_InsertCustomerGroup", param: customerGroup, commandType: CommandType.StoredProcedure);
                return rowsAffect;
            }
        }
        /// <summary>
        /// Lấy tất cả khách hàng
        /// </summary>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public IEnumerable<Customer> GetAllCustomers()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var data = dbConnection.Query<Customer>($"Proc_GetCustomers", commandType: CommandType.StoredProcedure);
                return data;
            }
        }
        /// <summary>
        /// Lấy tất cả nhóm khách hàng
        /// </summary>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public IEnumerable<CustomerGroup> GetAllCustomerGroups()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var data = dbConnection.Query<CustomerGroup>($"Proc_GetCustomerGroups", commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}
