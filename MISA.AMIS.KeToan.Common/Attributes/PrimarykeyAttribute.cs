using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Attributes
{

    /// <summary>
    /// Lấy ra Attribute Email để kiểm tra định dạng Email
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailNotInvali : Attribute { }

    /// <summary>
    /// Khóa chính
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Primarykey : Attribute
    {
       
    }

}
