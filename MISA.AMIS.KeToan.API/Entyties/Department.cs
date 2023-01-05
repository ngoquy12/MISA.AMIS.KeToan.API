namespace MISA.AMIS.KeToan.API.Entyties
{
    /// <summary>
    /// Các trường liên quan đến phòng ban 
    /// </summary>
    public class Department
    {
        /// <summary>
        /// ID của phòng ban
        /// </summary>
        public Guid DepartmentID { get; set; }
        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Ngày thêm
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người thêm
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Thời gian chỉnh sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người chỉnh sưa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
