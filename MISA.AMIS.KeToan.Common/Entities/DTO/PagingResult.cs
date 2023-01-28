using MISA.AMIS.KeToan.Common;

namespace MISA.AMIS.KeToan.Common.Entities.DTO
{
    /// <summary>
    /// Kết quả trả về API tìm kiếm nhân viên theo bộ lọc và phân trang
    /// </summary>
    public class PagingResult
    {
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public double TotalPage { get; set; }
        /// <summary>
        /// Danh sách nhân viên tìm thấy
        /// </summary>
        public List<Employee> Data { get; set; }
        /// <summary>
        /// Tổng số bản ghi 
        /// </summary> 
        public double TotalRecord { get; set; }
    }
}

