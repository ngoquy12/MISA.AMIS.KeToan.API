using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// 
        /// Lấy thông tin tất cả  bản ghi 
        /// </summary>
        /// <returns>Danh sách bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
         IEnumerable<T> GetAllRecord();

        /// <summary>
        /// Lấy thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id của bản ghi muốn lấy</param>
        /// <returns>Thông tin một bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
         T GetRecordById(Guid recordId);

        /// <summary>
        /// Xoá thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id của bản ghi muốn xóa</param>
        /// <returns>1: Nếu xóa thành công, 0: Nếu xóa thất bại</returns>
        /// Created by: NVQUY(05/01/2023)
        public int DeleteRecordById(Guid recordId);

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <returns>1: Nếu thêm mới thành công, 0: Nếu thêm mới thất bại</returns>
        /// Created By: NVQUY(05/01/2023)
        public ServiceResponse InsertRecord(T entity);

        ///// <summary>
        ///// Cập nhật một bản ghi theo Id
        ///// </summary>
        ///// <returns>1: Nếu cập nhật thành công, 0: Nếu cập nhật thất bại</returns>
        ///// Created By: NVQUY(05/01/2023)
        public ServiceResponse UpdateRecord(T entity, Guid entityId);

        /// <summary>
        /// Xóa nhiều bản ghi theo danh sách Id
        /// </summary>
        /// <param name="entityId">Danh sách Id </param>
        /// <returns>1: Nếu xóa thành công, 0: Nêú xóa thất bại</returns>
        public int DeleteMultiPath(ListRecordId listRecordId);

    }
}
