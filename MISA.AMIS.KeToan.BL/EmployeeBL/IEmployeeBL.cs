using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.DL;

namespace MISA.AMIS.KeToan.BL
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {
        /// <summary>
        /// API lấy mã mới
        /// Author: NVQUY(25/12/2022)
        /// </summary>
        /// <returns>Mã nhân viên mới</returns> 
        public string GetEmployeeNewCode();

        /// <summary>
        /// API lấy danh sách nhân viên theo bộ lọc và phân trang
        /// Author : NVQUY(25/12/2022
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <returns></returns>
        public PagingResult GetEmployeeByFilterAndPaging(string? keyword, int pageSize = 10, int pageNumber = 1);

    }
}
