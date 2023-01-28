namespace MISA.AMIS.KeToan.Common.Entities.DTO
{
    public class ServiceResponse
    {
        /// <summary>
        /// Trạng thái phản hồi thành công hay thất bại
        /// </summary> 
        public int Success { get; set; }

        /// <summary>
        /// Dữ liệu trả về sau khi thành công hoặc thất bại
        /// </summary>
        public object? Data { get; set; }
    }
}
