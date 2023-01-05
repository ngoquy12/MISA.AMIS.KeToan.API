namespace MISA.AMIS.KeToan.API.Entyties.DTO
{
    public class ServiceResponse
    {
        /// <summary>
        /// Trạng thái phản hồi
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Dữ liệu trả về sau khi đã Validate
        /// </summary>
        public object Data { get; set; }
    }
}
