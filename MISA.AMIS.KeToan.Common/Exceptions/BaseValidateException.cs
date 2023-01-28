using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Exceptions
{
    public class BaseValidateException : Exception
    {
        string? MessageErrorValidate = null;

        public BaseValidateException(string errorMessage)
        {
            this.MessageErrorValidate= errorMessage;
        }
        public override string Message
        {
            get
            {
                return MessageErrorValidate;
            }
        }
    }
}
