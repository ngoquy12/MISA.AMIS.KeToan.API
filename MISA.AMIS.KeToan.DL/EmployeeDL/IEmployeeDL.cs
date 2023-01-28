using Dapper;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;

namespace MISA.AMIS.KeToan.DL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// API lấy danh sách nhân viên theo bộ lọc và phân trang
        /// Author : NVQUY(25/12/2022
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <returns></returns>
        public PagingResult GetEmployeeByFilterAndPaging(string? keyword, int pageSize = 10, int pageNumber = 1);

        /// <summary>
        /// API lấy mã mới
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <returns>Mã nhân viên mới</returns> 
        public string GetEmployeeNewCode();

        /// <summary>
        /// Hàm kiểm tra trùng mã 
        /// Author: NVQUY(31/12/2022)
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên nhập vào từ người dùng</param>
        /// <returns>True: Nếu mã người dùng nhập vào trùng với mã đã tồn tại trong database, False: Nếu hai mã không trùng nhau</returns>
        public bool CheckEmployeeCode(string employeeCode, Guid employeeId);

    }

}
