using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.API.Entyties;
using MISA.AMIS.KeToan.API.Enums;
using MySqlConnector;
using System.Text.RegularExpressions;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeUnitTestsController : ControllerBase
    {
        /// <summary>
        /// API thêm mới nhân viên
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>
        /// 201: Thêm mới thành công
        /// 400: Dữ liệu nhập vào không hợp lệ
        /// 500: Lỗi exception
        /// </returns>
        [HttpPost]
        public int InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                //Khai báo các thông tin cần thiết
                var error = new ErrorService();
                var errorData = new Dictionary<string, string>();
                var errorMsgs = new List<string>();
                //Bước 1:Validate dữ liệu: trả về mã 400 kèm theo các thông báo lỗi
                //1.1. Kiểm tra mã nhân viên bắt buộc nhập
                if (string.IsNullOrEmpty(employee.EmployeeCode))
                {
                    return 400;
                }
                //1.2. Kiểm tra tên nhân viên bắt buộc nhập
                if (string.IsNullOrEmpty(employee.EmployeeName))
                {
                    return 400;
                }
                //1.3. Kiểm tra thông tin phòng ban không được phép để trống
                //if (Guid.Empty(employee.DepartmentID))
                //{
                //    errorData.Add("DepartmentID", Resources.ResourceVN.DepartmentNotEmpty);
                //    errorMsgs.Add(Resources.ResourceVN.DepartmentNotEmpty);
                //}
                //1.4. Kiểm  tra định dạng Email
                if (!EmailIsValid(employee.Email))
                {
                    return 400;
                }

                //1.5. Kiểm tra trùng mã
                if (CheckEmployeeCode(employee.EmployeeCode))
                {
                    return 400;
                }
                //Chuẩn bị câu lệnh procedure
                var storedProcedureName = "Proc_employee_Insert";

                //Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                var newEmployeeID = Guid.NewGuid();
                var newDepartmentID = Guid.NewGuid();
                parameters.Add("@EmployeeID", newEmployeeID);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@Address", employee.Address);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssueDate", employee.IdentityIssueDate);
                parameters.Add("@IdentityIssuePlace", employee.IdentityIssuePlace);
                parameters.Add("@ContactTitle", employee.ContactTitle);
                parameters.Add("@ContactMobile", employee.ContactMobile);
                parameters.Add("@ContactFax", employee.ContactFax);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@BankAccount", employee.BankAccount);
                parameters.Add("@BankName", employee.BankName);
                parameters.Add("@BankBranchName", employee.BankBranchName);
                parameters.Add("@CreatedDate", employee.CreatedDate);
                parameters.Add("@CreatedBy", employee.CreatedBy);
                parameters.Add("@ModifiedDate", employee.ModifiedDate);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                //Khởi tạo kết nối đến database
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Gọi vào database để chạy stored procedure  trên
                var result = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                //Nếu thành công
                if (result > 0)
                {
                    return 201;
                }

                return 500;
                //Xử lý try catch
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 500;
            }

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
        /// Hàm xử lý Exception
        /// Author: NVQUY(31/12/2022)
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Đối tượng gồm các thông tin lỗi </returns>
        private IActionResult HandleException(Exception ex)
        {
            Console.WriteLine(ex);
            var error = new ErrorService();
            error.ErrorCode = ErrorCode.ExceptionCode;
            error.DevMsg = ex.Message;
            error.UserMsg = Resources.ResourceVN.Error_Exception;
            error.Data = ex.Data;
            error.TraceId = HttpContext.TraceIdentifier;
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }

        /// <summary>
        /// Hàm kiểm tra trùng mã 
        /// Author: NVQUY(31/12/2022)
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên nhập vào từ người dùng</param>
        /// <returns>True: Nếu mã người dùng nhập vào trùng với mã đã tồn tại trong database, False: Nếu hai mã không trùng nhau</returns>
        private bool CheckEmployeeCode(string employeeCode)
        {
            try
            {
                //Chuẩn bị câu lệnh SQL
                var storedProcedureName = "Proc_employee_GetEmployeeCode";

                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị tham số đầu vào 
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeCode", employeeCode);

                //Thực hiện gọi vào DB
                var result = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                //Xử lý kết quả trả về 
                //Nếu thành công
                if (result != null)
                {
                    return true;
                }
                //Nếu không tìm thấy kết quả
                return false;
                //Try catch exception
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
