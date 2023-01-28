using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common.Resources;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.DL;
using MISA.AMIS.KeToan.Common.Constants;
using System.Resources;

namespace MISA.AMIS.KeToan.API.Controllers
{
    public class EmployeesController : BasesController<Employee>
    {
        #region Field

        private IEmployeeBL _employeeBL;

        #endregion

        #region Constructor

        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }

        #endregion

        #region Method
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
                var records = _employeeBL.GetEmployeeByFilterAndPaging(keyword, pageSize, pageNumber);

                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }
                return StatusCode(StatusCodes.Status200OK, "");
                //Try catch exception
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// API lấy mã mới
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <returns>Mã nhân viên mới</returns> 
        [HttpGet("EmployeeNewCode")]
        public IActionResult GetEmployeeNewCode()
        {
            try
            {
                var employeeNewCode = _employeeBL.GetEmployeeNewCode();
                //Xử lý kết quả trả về

                return StatusCode(StatusCodes.Status200OK, employeeNewCode);

                //Xử lý try catch
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            var error = new ErrorService();
            error.ErrorCode = ErrorCode.ExceptionCode;
            error.DevMsg = ex.Message;
            error.UserMsg = ResourceVN.Error_Exception;
            error.Data = ex.Data;
            error.MoreInfo = ResourceVN.Error_MoreInfo;
            error.TraceId = HttpContext.TraceIdentifier;
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }

        #endregion
    }
}

