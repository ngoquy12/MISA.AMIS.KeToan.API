using MISA.AMIS.KeToan.API.Entyties;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using MISA.AMIS.KeToan.API.Enums;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// API lấy thông tin tất cả phòng ban
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <returns>Danh sách tất cả phòng ban</returns>
        [HttpGet]
        public IActionResult GetAllDepartment()
        {
            try
            {
                //Chuẩn bị câu lệnh SQL
                var proceduresName = "Proc_department_GetAll";

                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị tham số đầu vào 
                var departments = mySqlConnection.Query(proceduresName, commandType: System.Data.CommandType.StoredProcedure);
                //Thực hiện gọi vào DB

                //Xử lý kết quả trả về 
                return StatusCode(StatusCodes.Status200OK, departments);
              
                //Try catch exception
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// API lấy thông tin tất cả phòng ban theo ID
        /// Author : NQUY(25/12/2022)
        /// </summary>
        /// <param name="departmentID">Id của phòng ban</param>
        /// <returns>Thông tin một phòng ban</returns>
        [HttpGet("{departmentID}")]
        public IActionResult GetDepartmentByID([FromRoute] Guid departmentID)
        {
            try
            {
                //Khới tạo kết nối đến DB MySQL
                var connectionString = "Server=localhost;Port=3306;Database=misa.web11.tcdn1.nvquy;Uid=root;Pwd=22121944;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị câu lệnh SQL
                var proceduresName = "Proc_department_GetByID";
                //Chuẩn bị tham số đầu vào 
                var department = mySqlConnection.QueryFirstOrDefault(proceduresName, commandType: System.Data.CommandType.StoredProcedure);
                //Thực hiện gọi vào DB

                //Xử lý kết quả trả về 
                if (department != null)
                {
                    return StatusCode(StatusCodes.Status200OK, department);
                }

                return StatusCode(StatusCodes.Status404NotFound, "");

                //Try catch exception
            }
            catch (Exception ex)
            {
                return HandleException(ex);
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
    }
}
