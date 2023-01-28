using MISA.AMIS.KeToan.Common.Enums;
namespace MISA.AMIS.KeToan.Common.Entities
{
    //Danh sách các lỗi trả về
    public class ErrorService
    {
        //Mã code 
        public ErrorCode ErrorCode { get; set; }
        //Lỗi hiển thị cho dev
        public string DevMsg { get; set; }
        //Lỗi hiển thị cho người dùng
        public string UserMsg { get; set; }
        //Đối tượng data
        public object Data { get; set; }
        //Id lỗi 
        public string TraceId { get; set; }
        // Chi tiết lỗi
        public string MoreInfo { get; set; }

    }
}
