using MISA.AMIS.KeToan.Common.Attributes;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common.Exceptions;
using MISA.AMIS.KeToan.Common.Resources;
using MISA.AMIS.KeToan.DL;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace MISA.AMIS.KeToan.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field

        private IEmployeeDL _employeeDL;

        #endregion

        #region Constructor
        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Created by: NVQUY(08/01/2023)
        public string GetEmployeeNewCode()
        {
            return _employeeDL.GetEmployeeNewCode();
        }

        /// <summary>
        /// API lấy danh sách nhân viên theo bộ lọc và phân trang
        /// Author : NVQUY(25/12/2022
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <returns></returns>
        public PagingResult GetEmployeeByFilterAndPaging(string? keyword, int pageSize, int pageNumber)
        {
            return _employeeDL.GetEmployeeByFilterAndPaging(keyword, pageSize, pageNumber);
        }

        /// <summary>
        /// Hàm kiểm tra định dạng Email
        /// Author: NVQUY(31/12/2022)
        /// </summary>
        /// <param name="email">Chuỗi email nhập từ người dùng</param>
        /// <returns>True: Nếu đúng định dạng, False: Nếu sai định dạng</returns>
        public static bool EmailIsValid(string email)
        {
            try
            {
                string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

                if (Regex.IsMatch(email, expression))
                {
                    if (Regex.Replace(email, expression, string.Empty).Length == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra một chuỗi có phải là sóo hay không?
        /// </summary>
        /// <param name="pText">Chuỗi nhập vào </param>
        /// <returns>True: Nếu chuỗi là số, False: Nếu không phải là số</returns>
        public bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*.?[0-9]+$");
            return regex.IsMatch(pText);
        }

        /// <summary>
        /// Hàm validate dữ liệu riêng cho đối tượng Employee
        /// </summary>
        /// <param name="employee">Đối tượng Employee</param>
        protected override ServiceResponse ValidateCustom(Employee employee)
        {
            var validateFailures = new List<string>();
            //Kiểm tra trùng mã
            var isDuplicate = _employeeDL.CheckEmployeeCode(employee.EmployeeCode, employee.EmployeeID);
            if (isDuplicate == true)
            {
                validateFailures.Add(ResourceVN.EmployeeCodeNotDuplicate);
            }

            //Kiểm tra trường hợp mã nhân viên dài quá 20 kí tự
            if (!string.IsNullOrEmpty(employee.EmployeeCode) && employee.EmployeeCode.ToString().Length > 20)
            {
                validateFailures.Add("Mã nhân viên không được dài quá 20 kí tự");
            }

            //Kiểm tra kí tự cuối cùng của mã nhân viên có phải số không
            if (!string.IsNullOrEmpty(employee.EmployeeCode))
            {
                var endEmployeeCode = employee.EmployeeCode.Substring(employee.EmployeeCode.Length - 1, 1);
                var result = IsNumber(endEmployeeCode);
                if (result == false)
                {
                    validateFailures.Add(ResourceVN.EmployeeCodeLastIsNumner);
                }
            }

            //Ngày sinh không được lớn hơn ngày hiện tại
            if (employee.DateOfBirth > DateTime.Now)
            {
                validateFailures.Add(ResourceVN.Error_DateOfBirth);
            }

            //Ngày cấp không được lớn hơn ngày hiện tại
            if (employee.IdentityIssueDate > DateTime.Now)
            {
                validateFailures.Add(ResourceVN.Error_IdentityIssuDate);
            }

            // Kiểm tra định dạng Email
            if (!string.IsNullOrEmpty(employee.Email))
            {
                if (!EmailIsValid(employee.Email))
                {
                    validateFailures.Add(ResourceVN.EmailInvalidate);
                }
            }

            //Kiểm tra số lượng lỗi
            if (validateFailures.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = (int)StatusRespone.Failure,
                    Data = new ErrorService
                    {
                        ErrorCode = ErrorCode.InvaliteData,
                        UserMsg = String.Join(", ", validateFailures.ToArray()[0]),
                        DevMsg = ResourceVN.DevMsg_InvalidData,
                        MoreInfo = ResourceVN.Error_MoreInfo,
                        Data = validateFailures
                    }
                };
            }
            return new ServiceResponse
            {
                Success = (int)StatusRespone.Successful
            };
        }

        /// <summary>
        /// Lấy ra kid tự đầu và kí tự cuối của mã nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên</param>
        /// Created by: NVQUY(12/01/2022)

        protected virtual void BeforeInsert(ref Employee employee)
        {
            employee.StartEmployeeCode = Regex.Replace(employee.EmployeeCode, @"\d", "");

            employee.EndEmployeeCode = int.Parse(Regex.Replace(employee.EmployeeCode, @"\D", ""));
        }
    }
    #endregion
}
