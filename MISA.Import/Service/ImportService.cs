using Microsoft.AspNetCore.Http;
using MISA.Import.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.Service
{
    public class ImportService
    {
        DBService db = new DBService();
        /// <summary>
        /// Thực hiện nhập khẩu và đổ dữ liệu ra list
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// CreatedBy KDLong 06/05/2021
        public IEnumerable<Customer> Import(IFormFile formFile, CancellationToken cancellationToken)
        {

            if (formFile == null || formFile.Length <= 0)
            {
                return null;
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var list = new List<Customer>();

            using (var stream = new MemoryStream())
            {
                formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;

                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    var customers = db.GetAllCustomers();
                    var customerGroups = db.GetAllCustomerGroups();

                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 3; row <= rowCount; row++)
                    {

                        var status = "";

                        var customerCode = (string)worksheet.Cells[row, 1].Value;
                        var phoneNumber = (string)worksheet.Cells[row, 5].Value;

                        var checkCode = true;
                        var checkPhoneNumber = true;
                        foreach (var customer in customers)
                        {
                            if (customer.CustomerCode == customerCode && checkCode == true)
                            {
                                status += "Mã khách hàng đã trùng với khách hàng khác trong hệ thống.";
                                checkCode = false;
                            }
                            if (customer.PhoneNumber == phoneNumber && checkPhoneNumber == true)
                            {
                                status += "SĐT đã có trong hệ thống.";
                                checkPhoneNumber = false;
                            }
                            if (checkPhoneNumber == false && checkCode == false) break;
                        }
                        checkCode = true;
                        checkPhoneNumber = true;
                        foreach (var customer in list)
                        {
                            if (customer.CustomerCode == customerCode && checkCode == true)
                            {
                                status += "Mã khách hàng đã trùng với khách hàng khác trong tệp nhập khẩu.";
                                checkCode = false;
                            }
                            if (customer.PhoneNumber == phoneNumber && checkPhoneNumber == true)
                            {
                                status += "SĐT đã có trong tệp nhập khẩu.";
                                checkPhoneNumber = false;
                            }
                            if (checkPhoneNumber == false && checkCode == false) break;
                        }

                        var customerName = (string)worksheet.Cells[row, 2].Value;
                        var memberCardCode = (string)worksheet.Cells[row, 3].Value;
                        var customerGroupName = (string)worksheet.Cells[row, 4].Value;
                        var isCustomerGroupNameExist = false;
                        foreach (var customerGroup in customerGroups)
                        {
                            if (customerGroup.CustomerGroupName == customerGroupName)
                            {
                                isCustomerGroupNameExist = true;
                                break;
                            }
                        }
                        if (!isCustomerGroupNameExist)
                        {
                            status += "Nhóm khách hàng không có trong hệ thống.";
                        }

                        var dateOfBirth = "";
                        if (worksheet.Cells[row, 6].Value == null)
                        {
                            dateOfBirth = "";
                        }
                        else
                        if (worksheet.Cells[row, 6].Value.GetType() == typeof(double))
                        {
                            dateOfBirth = worksheet.Cells[row, 6].Value.ToString().Trim();
                        }
                        else dateOfBirth = (string)worksheet.Cells[row, 6].Value;


                        var fmDate = FormatDate(dateOfBirth);
                        var companyName = (string)worksheet.Cells[row, 7].Value;
                        var taxCode = worksheet.Cells[row, 8].Value.ToString().Trim();
                        var email = (string)worksheet.Cells[row, 9].Value;
                        var address = (string)worksheet.Cells[row, 10].Value;
                        var note = (string)worksheet.Cells[row, 11].Value;

                        list.Add(new Customer
                        {
                            CustomerCode = customerCode,
                            CustomerName = customerName,
                            MemberCardCode = memberCardCode,
                            CustomerGroupName = customerGroupName,
                            PhoneNumber = phoneNumber,
                            DateOfBirth = fmDate,
                            CompanyName = companyName,
                            TaxCode = taxCode,
                            Email = email,
                            Address = address,
                            Note = note,
                            Status = status,
                        });
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Format Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>
        /// Date
        /// Vd:12/2021 =>01/12/2021
        /// Vd:2021=>01/01/2021
        /// </returns>
        /// CreatedBy KDLong 06/05/2021
        private DateTime FormatDate(string date)
        {
            DateTime res = new DateTime();
            Regex rg1 = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
            Regex rg2 = new Regex(@"^(0[1-9]{1}|1[0-2]{1})/\d{4}$");
            Regex rg3 = new Regex(@"\d{4}$");
            if (rg1.IsMatch(date))
            {
                DateTime res1 = DateTime.ParseExact(date, "dd/MM/yyyy", null);
                return res1;
            }
            else if (rg2.IsMatch(date))
            {
                string newDate = "01/" + date;
                DateTime res1 = DateTime.ParseExact(newDate, "dd/MM/yyyy", null);
                return res1;
            }
            else if (rg3.IsMatch(date))
            {
                string newDate = "01/01/" + date;
                DateTime res1 = DateTime.ParseExact(newDate, "dd/MM/yyyy", null);
                return res1;
            }
            return res;
        }
    }
}
