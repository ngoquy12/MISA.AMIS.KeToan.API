using MISA.AMIS.KeToan.API.Enums;

namespace MISA.AMIS.KeToan.API.Entyties
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

    }
}
