namespace MISA.AMIS.KeToan.API.Enums
{
    public enum ErrorCode
    {
        /// <summary>
        /// Lỗi xảy ra exception
        /// </summary>
        ExceptionCode = 1,
        /// <summary>
        /// Lỗi trùng mã
        /// </summary>
        DuplicateCode = 2,
        /// <summary>
        /// Lỗi xử lý dữ liệu đầu vào
        /// </summary>
        InvaliteData = 3
    }
}
