using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL.UnitTests
{
    public class FakeBaseDL<T> : IBaseDL<T>
    {
        public int DeleteRecordById(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllRecords()
        {
            throw new NotImplementedException();
        }

        public T GetRecordById(Guid entityId)
        {
            return default;
        }

        public int InsertRecord(T entity)
        {
            throw new NotImplementedException();
        }

        public int UpdateRecord(T entity, Guid entityId)
        {
            throw new NotImplementedException();
        }
    }
}
