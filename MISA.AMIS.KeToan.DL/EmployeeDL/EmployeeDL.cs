using Dapper;
using MISA.AMIS.KeToan.Common.Constants;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MySqlConnector;

namespace MISA.AMIS.KeToan.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {

        /// <summary>
        /// Hàm kiểm tra trùng mã 
        /// Author: NVQUY(31/12/2022)
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên nhập vào từ người dùng</param>
        /// <returns>True: Nếu mã người dùng nhập vào trùng với mã đã tồn tại trong database, False: Nếu hai mã không trùng nhau</returns>
        public bool CheckEmployeeCode(string employeeCode, Guid employeeId)
        {
            //Chuẩn bị câu lệnh SQL
            var storedProcedureName = Procedure.CHECK_DUPLICATE_CODE;

            //Chuẩn bị tham số đầu vào 
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeCode", employeeCode);
            parameters.Add("@EmployeeID", employeeId);
            var result = "";

            //Khới tạo kết nối đến DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Thực hiện gọi vào DB
                result = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            //Xử lý kết quả trả về 
            //Nếu thành công
            if (result != null)
            {
                return true;
            }
            //Nếu không tìm thấy kết quả
            return false;
        }

        /// <summary>
        /// API lấy danh sách nhân viên theo bộ lọc và phân trang
        /// Author : NVQUY(05/01/2023)
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <returns></returns>
        public PagingResult GetEmployeeByFilterAndPaging(string? keyword, int pageSize = 10, int pageNumber = 1)
        {
            //Chuẩn bị câu lệnh SQL
            var storedProcedureName = Procedure.SEARCH_AND_PAGING;

            //Chuẩn bị tham số đầu vào 
            var parameters = new DynamicParameters();
            parameters.Add("@Keyword", keyword);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@PageNumber", pageNumber);

            //Khới tạo kết nối đến DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            //Thực hiện gọi vào DB
            using (var employees = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure))
            {
                var totalRecord = employees.Read<int>().FirstOrDefault();
                var listEmployee = employees.Read<Employee>().ToList();

                var totalPage = Math.Ceiling((double)totalRecord / pageSize);

                //Xử lý kết quả trả về 
                return new PagingResult
                {
                    TotalPage = totalPage,
                    TotalRecord = totalRecord,
                    Data = listEmployee
                };
            }

        }

        /// <summary>
        /// API lấy mã mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns> 
        /// Created by:  NVQUY(05/01/2023)
        public string GetEmployeeNewCode()
        {
            //Chuẩn bị câu lệnh procedure
            var storedProcedureName = Procedure.GET_NEW_CODE;
            var employeeNewCode = "";

            //Khởi tạo kết nối đến database
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Gọi vào database để chạy stored procedure  trên
                employeeNewCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
            }
                //Xử lý kết quả trả về
                return employeeNewCode;
        }
    }
}

