using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Enums
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
