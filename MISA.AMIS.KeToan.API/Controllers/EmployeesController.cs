using MISA.AMIS.KeToan.API.Entyties;
using MISA.AMIS.KeToan.API.Entyties.DTO;
using MISA.AMIS.KeToan.API.Enums;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        /// <summary>
        /// API Lây danh sách tất cả nhân viên
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <returns>Danh sách tất cả nhân viên</returns>
        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            try
            {

                //Chuẩn bị câu lệnh SQL
                var proceduresName = "Proc_employee_GetAll";

                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị tham số đầu vào 
                var employees = mySqlConnection.Query(proceduresName, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về 
                return StatusCode(StatusCodes.Status200OK, employees);

                //Try catch exception
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// API lấy thông tin một nhân viên theo ID
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn lấy</param>
        /// <returns>Thông tin một nhân viên muốn lấy</returns>
        [HttpGet("{employeeID}")]
        public IActionResult GetEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                //Chuẩn bị câu lệnh SQL
                var storedProcedureName = "Proc_employee_GetByID";

                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị tham số đầu vào 
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);

                //Thực hiện gọi vào DB
                var employee = mySqlConnection.QueryFirstOrDefault<Employee>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                //Xử lý kết quả trả về 
                //Nếu thành công
                if (employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
                }
                //Nếu không tìm thấy kết quả
                return StatusCode(StatusCodes.Status404NotFound, "");
                //Try catch exception
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// API lấy danh sách nhân viên theo bộ lọc và phân trang
        /// Author : NVQUY(25/12/2022
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <returns></returns>
        [HttpGet("filter")]
        public IActionResult GetEmployeeByFilterAndPaging(
          [FromQuery] string? keyword,
          [FromQuery] int pageSize = 10,
          [FromQuery] int pageNumber = 1
          )
        {
            try
            {
                //Chuẩn bị câu lệnh SQL
                var storedProcedureName = "Proc_employee_SearchAndPaging";

                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị tham số đầu vào 
                var parameters = new DynamicParameters();
                parameters.Add("@Keyword", keyword);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageNumber", pageNumber);

                //Thực hiện gọi vào DB
                var employees = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về 
                var totalRecord = employees.Read<int>().FirstOrDefault();
                var listEmployee = employees.Read<Employee>().ToList();

                var totalPage = Math.Ceiling((double)totalRecord / pageSize);

                //Nếu thành công
                return StatusCode(StatusCodes.Status200OK, new PagingResult
                {
                    Data = listEmployee,
                    TotalRecord = totalRecord,
                    TotalPage = totalPage
                });
                //Try catch exception
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// API lấy mã mới
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <returns>Mã nhân viên mới</returns> 

        [HttpGet("newEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                //Chuẩn bị câu lệnh SQL
                var storedProcedureName = "Proc_employee_GetMaxCode";

                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Thực hiện gọi vào DB
                var employeeNewCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                //Xử lý kết quả trả về 
                //Nếu thành công
                return StatusCode(StatusCodes.Status200OK, employeeNewCode);

                //Try catch exception
            }
            catch (Exception ex)
            {
                return HandleException(ex);

            }
        }

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
        public IActionResult InsertEmployee([FromBody] Employee employee)
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
                    errorData.Add("EmployeeCode", Resources.ResourceVN.EmployeeCodeNotEmpty);
                    errorMsgs.Add(Resources.ResourceVN.EmployeeCodeNotEmpty);
                }
                //1.2. Kiểm tra tên nhân viên bắt buộc nhập
                if (string.IsNullOrEmpty(employee.EmployeeName))
                {
                    errorData.Add("EmployeeName", Resources.ResourceVN.EmployeeNameNotEmpty);
                    errorMsgs.Add(Resources.ResourceVN.EmployeeNameNotEmpty);
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
                    errorData.Add("Email", Resources.ResourceVN.EmailInvalidate);
                    errorMsgs.Add(Resources.ResourceVN.EmailInvalidate);
                }

                //1.5. Kiểm tra trùng mã
                if (CheckEmployeeCode(employee.EmployeeCode))
                {
                    errorData.Add("EmployeeCode", Resources.ResourceVN.EmployeeCodeNotDuplicate);
                    errorMsgs.Add(Resources.ResourceVN.EmployeeCodeNotDuplicate);
                }

                //Kiểm tra số lượng lỗi
                if (errorData.Count > 0)
                {
                    error.UserMsg = Resources.ResourceVN.DataNotInvalidate;
                    error.Data = errorMsgs;
                    return BadRequest(error);
                }

                //Chuẩn bị câu lệnh procedure
                var storedProcedureName = "Proc_employee_Insert";

                //Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                var newEmployeeID = Guid.NewGuid();
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
                    return StatusCode(StatusCodes.Status201Created, newEmployeeID);
                }
                else
                {
                    return Ok(result);
                }
                //Xử lý try catch
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }

        /// <summary>
        /// API sửa thông tin nhân viên theo ID
        /// Author : NVQUY(25/12/2022)
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        [HttpPut("{employeeID}")]
        public IActionResult UpdateEmpoyee([FromRoute] Guid employeeID, [FromBody] Employee employee)
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
                    errorData.Add("EmployeeCode", Resources.ResourceVN.EmployeeCodeNotEmpty);
                    errorMsgs.Add(Resources.ResourceVN.EmployeeCodeNotEmpty);
                }
                //1.2. Kiểm tra tên nhân viên bắt buộc nhập
                if (string.IsNullOrEmpty(employee.EmployeeName))
                {
                    errorData.Add("EmployeeName", Resources.ResourceVN.EmployeeNameNotEmpty);
                    errorMsgs.Add(Resources.ResourceVN.EmployeeNameNotEmpty);
                }
                //1.3. Kiểm tra thông tin phòng ban không được phép để trống
                if (employee.DepartmentID == null)
                {
                    errorData.Add("DepartmentID", Resources.ResourceVN.DepartmentNotEmpty);
                    errorMsgs.Add(Resources.ResourceVN.DepartmentNotEmpty);
                }
                //1.4. Kiểm  tra định dạng Email
                if (!EmailIsValid(employee.Email))
                {
                    errorData.Add("Email", Resources.ResourceVN.EmailInvalidate);
                    errorMsgs.Add(Resources.ResourceVN.EmailInvalidate);
                }

                //Kiểm tra số lượng lỗi
                if (errorData.Count > 0)
                {
                    error.UserMsg = Resources.ResourceVN.DataNotInvalidate;
                    error.Data = errorMsgs;
                    return BadRequest(error);
                }

                //Chuẩn bị câu lệnh procedure
                var storedProcedureName = "Proc_employee_UpdateByID";

                //Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employee.EmployeeID);
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
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return Ok(result);
                }
                //Xử lý try catch
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// API xóa thông tin nhân viên theo ID
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        [HttpDelete("{employeeID}")]
        public IActionResult DeleteEmployee([FromRoute] Guid employeeID)
        {
            try
            {
                //Chuẩn bị câu lệnh SQL
                var proceduresName = "Proc_employee_DeleteByID";
                //Chuẩn bị tham số đầu vào 
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);

                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Thực hiện gọi vào DB
                var result = mySqlConnection.Execute(proceduresName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về 
                //Nếu thành công thì sẽ trả về id của nhân viên vừa xóa
                if (result > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                //Nếu không tìm thấy id thì  trả về rỗng
                return StatusCode(StatusCodes.Status404NotFound, "");

                //Try catch exception
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA để được trợ giúp.",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });
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
        /// <summary>
        /// Hàm xử lý dữ liệu nhập vào từ người dùng 
        /// Author: NVQUY(05/01/2023)
        /// </summary>
        /// <param name="employee">Dữ liệu trả vào từ người dùng</param>
        /// <returns>True: Nếu có dữ liệu nhập vào, False:Nếu không có dữ liệu nhập vào từ người dùng</returns>
        private ServiceResponse ValidateRequestData(Employee employee)
        {
            //Khai báo thông tin cuar tấ
            var properties = typeof(Employee).GetProperties();
            var validateFailures = new List<string>();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(employee);
                var requiredAttribute = (RequiredAttribute?)Attribute.GetCustomAttribute(property,
                    typeof(RequireHttpsAttribute));
                if (requiredAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailures.Add(requiredAttribute.ErrorMessage);
                }
            }
            if (validateFailures.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = validateFailures
                };
            }
            return new ServiceResponse
            {
                Success = true
            };
        }
    }
}

