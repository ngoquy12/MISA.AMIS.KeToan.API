using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common.Exceptions;
using MISA.AMIS.KeToan.Common.Resources;
using MISA.AMIS.KeToan.DL;
using MySqlConnector;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion


        #region Constructor
        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }
        #endregion

        /// <summary>
        /// Lấy thông tin một bản ghi theo Id
        /// </summary>
        /// <param name="recordId">Id của bản ghi cần lấyI</param>
        /// <returns>Thông tin của bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
        [HttpGet("{entitydId}")]
        public IActionResult GetRecordById([FromRoute] Guid entitydId)
        {
            try
            {
                //Khai báo hàm lấy thông tin nhân viên theo Id dưới tầng BL
                var record = _baseBL.GetRecordById(entitydId);
                //Kiểm tra giá trị trả về
                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin tất cả bản ghi
        /// </summary>
        /// <returns>Thông tin của tất cả bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
        [HttpGet]
        public IActionResult GetAllRecord()
        {
            try
            {
                var records = _baseBL.GetAllRecord();

                return StatusCode(StatusCodes.Status200OK, records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xóa thông tin một bản ghi theo Id
        /// </summary>
        /// <returns>1: Nếu xóa thành công, 0: Nếu xóa thất bại</returns>
        /// Created by: NVQUY(05/01/2023)
        [HttpDelete("{entitydId}")]
        public IActionResult DeleteRecordById([FromRoute] Guid entitydId)
        {
            try
            {
                var result = _baseBL.DeleteRecordById(entitydId);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Thêm mới một bản ghi 
        /// </summary>
        /// <returns>1: Nếu thêm thành công, 0: Nếu thêm thất bại</returns>
        /// Created by: NVQUY(05/01/2023)
        [HttpPost]
        public IActionResult InsertRecord(T entity)
        {
            try
            {
                var result = _baseBL.InsertRecord(entity);

                if (result.Success == (int)StatusRespone.Failure)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

                return StatusCode(StatusCodes.Status201Created, result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin một ban ghi theo Id
        /// </summary>
        /// <returns>1: Nếu cập nhật  thành công, 0: Nếu cập nhật thất bại</returns>
        /// Created by: NVQUY(05/01/2023)
        [HttpPut("{entityId}")]
        public IActionResult UpdateRecord(T entity, Guid entityId)
        {
            try
            {
                var result = _baseBL.UpdateRecord(entity, entityId);

                if (result.Success == (int)StatusRespone.Failure)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

                return StatusCode(StatusCodes.Status200OK, result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi theo Id
        /// </summary>
        /// <param name="listRecordId">Danh sách Id</param>
        /// <returns>1: Nếu xóa thành công, 0: Nếu xóa thất bại</returns>
        [HttpPost("DeleteBatch")]
        public IActionResult DeleteMultipleEmployees([FromBody] ListRecordId listEmployeeID)
        {
            //Khởi tạo kết nối tới DB
            string connectionString = DatabaseContext.ConnectionString;
            var mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open();
            //Chuẩn bị câu lệnh SQL
            var storeProcedureName = "Proc_employee_DeleteByID";

            var trans = mySqlConnection.BeginTransaction();

            try
            {
                //Chuẩn bị tham số đầu vào
                foreach (var employeeID in listEmployeeID.PathRecordId)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeID", employeeID);
                    mySqlConnection.Execute(storeProcedureName, parameters, trans, commandType: System.Data.CommandType.StoredProcedure);
                }
                trans.Commit();

                return StatusCode(200, listEmployeeID.PathRecordId.Count);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Hàm xử lý các lỗi liên quan đến exception
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Lỗi 500 kèm danh sách lỗi</returns>
        private IActionResult HandleException(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorService
            {
                ErrorCode = ErrorCode.ExceptionCode,
                DevMsg = ex.Message,
                UserMsg = ResourceVN.DataNotInvalidate,
                Data = ex.Data,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}
