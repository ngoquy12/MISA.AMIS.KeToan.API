using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.DL;

namespace MISA.AMIS.KeToan.BL
{
    public class DepartmentBL : BaseBL<Department>, IDepartmentBL
    {

        #region Field

        private IDepartmentDL _departmentDL;

        #endregion


        #region Constructor
        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion
    }
}
