using MISA.AMIS.KeToan.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// 
        /// Lấy thông tin tất cả  bản ghi 
        /// </summary>
        /// <returns>Danh sách bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// Lấy thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id của bản ghi muốn lấy</param>
        /// <returns>Thông tin một bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
        public T GetRecordById(Guid entityId);

        /// <summary>
        /// Xoá thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id của bản ghi muốn xóa</param>
        /// <returns>Id của bản ghi vừa xóa</returns>
        /// Created by: NVQUY(05/01/2023)
        public int DeleteRecordById(Guid entityId);

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <returns>1: Nếu thêm mới thành công, 0: Nếu thêm mới thất bại</returns>
        /// Created By: NVQUY(05/01/2023)
        public int InsertRecord(T entity);

        /// <summary>
        /// Cập nhật một bản ghi theo Id
        /// </summary>
        /// <returns>1: Nếu cập nhật thành công, 0: Nếu cập nhật thất bại</returns>
        /// Created By: NVQUY(05/01/2023)
        public int UpdateRecord(T entity, Guid entityId);

        /// <summary>
        /// Xóa nhiều bản ghi theo danh sách Id
        /// </summary>
        /// <param name="entityId">Danh sách Id </param>
        /// <returns>1: Nếu xóa thành công, 0: Nêú xóa thất bại</returns>
        public int DeleteMultiPath(ListRecordId listRecordId);

       
    }
}
