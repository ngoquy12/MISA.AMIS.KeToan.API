using MISA.AMIS.KeToan.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MISA.AMIS.KeToan.Common.Entities
{
    /// <summary>
    /// Các trường liên quan đến phòng ban 
    /// </summary>
    public class Department :BaseEntity
    {
        #region Property
        /// <summary>
        /// ID của phòng ban
        /// </summary>
        [Primarykey]
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        [Required(ErrorMessage ="Mã phòng ban không được phép để trống")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [Required(ErrorMessage ="Tên phòng ban không được phép để trống")]
        public string DepartmentName { get; set; }

        #endregion


    }
}


