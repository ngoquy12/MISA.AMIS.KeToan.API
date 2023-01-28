using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Constants
{
    public class Procedure
    {
        /// <summary>
        /// Format tên procedure lấy thông tin tất cả bản ghi
        /// </summary>
        public static string GET_ALL = "Proc_{0}_GetAll";

        /// <summary>
        /// Format tên procedure lấy thông tin một bản ghi theo Id
        /// </summary>
        public static string GET_BY_ID = "Proc_{0}_GetByID";

        /// <summary>
        /// Format tên procedure xóa thông tin một bản ghi theo Id
        /// </summary>
        public static string DELETE_BY_ID = "Proc_{0}_DeleteByID";

        /// <summary>
        /// Format tên procedure thêm mới một bản ghi
        /// </summary>
        public static string INSERT_RECORD = "Proc_{0}_Insert";

        /// <summary>
        /// Format tên procedure lấy thông tin nhân viên theo tìm kiếm và phân trang
        /// </summary>
        public static string SEARCH_AND_PAGING = "Proc_employee_SearchAndPaging";

        /// <summary>
        /// Format tên procedure cập nhật một nhân viên theo ID
        /// </summary>
        public static string UPDATE_BY_ID = "Proc_{0}_UpdateByID";

        /// <summary>
        /// Format tên procedure kiểm tra trùng mã
        /// </summary>
        public static string CHECK_DUPLICATE_CODE = "Proc_employee_CheckCuplicateCode";

        /// <summary>
        /// Format tên procedure lấy mã mới
        /// </summary>
        public static string GET_NEW_CODE = "Proc_employee_GetMaxCode";

        /// <summary>
        /// Fomat tên procedure xóa nhiểu bản ghi theo id
        /// </summary>
        public static string DELETE_PATH_BY_ID = "Proc_{0}_DeleteByPathID";

    }
}
