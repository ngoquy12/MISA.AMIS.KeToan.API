namespace MISA.AMIS.KeToan.API.Entyties.DTO
{
    /// <summary>
    /// Kết quả trả về API tìm kiếm nhân viên theo bộ lọc và phân trang
    /// </summary>
    public class PagingResult
    {
        //Tổng số trang
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

