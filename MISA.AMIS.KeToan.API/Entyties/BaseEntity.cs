namespace MISA.AMIS.KeToan.API.Entyties
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người thêm
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Thời gian chỉnh sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        /// <summary>
        /// Người chỉnh sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
