using Dapper;
using MISA.AMIS.KeToan.Common.Attributes;
using MISA.AMIS.KeToan.Common.Constants;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MySqlConnector;

namespace MISA.AMIS.KeToan.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        /// <summary>
        /// Xóa nhiều bản ghi theo Id
        /// </summary>
        /// <param name="listRecordId">Danh sách Id </param>
        /// <returns>1: Nếu xóa thành công, 0: Nếu xóa thất bại</returns>
        public int DeleteMultiPath(ListRecordId listRecordId)
        {
            var className = typeof(T).Name;
            //Chuẩn bị câu lệnh SQL
            var proceduresName = string.Format(Procedure.DELETE_PATH_BY_ID, className);

            var parameters = new DynamicParameters();
            parameters.Add($"@{className}ID", listRecordId);

            //Khới tạo kết nối đến DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Thực hiện gọi vào DB
                var result = mySqlConnection.Execute(proceduresName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về 
                return result;
            }

        }

       

        /// <summary>
        /// Xoá thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id của bản ghi muốn xóa</param>
        /// <returns>Id của bản ghi vừa xóa</returns>
        /// Created by: NVQUY(05/01/2023)
        public int DeleteRecordById(Guid entityId)
        {
            var className = typeof(T).Name;
            //Chuẩn bị câu lệnh SQL
            var proceduresName = string.Format(Procedure.DELETE_BY_ID, className);

            var parameters = new DynamicParameters();
            parameters.Add($"@{className}ID", entityId);

            //Khới tạo kết nối đến DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Thực hiện gọi vào DB
                var result = mySqlConnection.Execute(proceduresName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về 
                return result;
            }
        }

        /// <summary>
        /// Lấy thông tin tất cả  bản ghi 
        /// </summary>
        /// <returns>Danh sách bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
        public IEnumerable<T> GetAllRecords()
        {
            string className = typeof(T).Name;
            //Chuẩn bị câu lệnh SQL
            var proceduresName = string.Format(Procedure.GET_ALL, className);

            //Khai báo kết quả trả về
            var records = new List<T>();

            //Khới tạo kết nối đến DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Chuẩn bị tham số đầu vào 
                records = (List<T>)mySqlConnection.Query<T>(proceduresName, commandType: System.Data.CommandType.StoredProcedure);

                return records;
            }
        }

        /// <summary>
        /// Lấy thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordID">Id của bản ghi muốn lấy</param>
        /// <returns>Thông tin một bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
        public T GetRecordById(Guid entityId)
        {
            var className = typeof(T).Name;
            //Chuẩn bị câu lệnh SQL
            var storedProcedureName = string.Format(Procedure.GET_BY_ID, className);

            //Khới tạo kết nối đến DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Chuẩn bị tham số đầu vào 
                var parameters = new DynamicParameters();
                parameters.Add($"@{className}ID", entityId);

                //Thực hiện gọi vào DB
                var record = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                //Xử lý kết quả trả về 
                return record;
            }
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="className">Đối tượng được thêm mới</param>
        /// <returns>1: Nếu thêm thành công, 0: Nếu thêm thất bại</returns>
        public int InsertRecord(T entity)
        {
            //Lấy tên đối tượng entity
            var className = typeof(T).Name;

            //Chuẩn bị câu lệnh SQL
            var storedProcedureName = string.Format(Procedure.INSERT_RECORD, className);

            //Chuẩn bị tham số đầu vào 
            var parameters = new DynamicParameters();

            //Lấy danh sách tất cả thuộc tính của class Employee
            var properties = typeof(T).GetProperties();

            //Lặp qua từng thuọc tính 
            foreach (var property in properties)
            {
                //Lấy ra giá trị của từng thuộc tính
                var propertyValue = property.GetValue(entity);

                //Lấy ra tên của từng thuộc tính
                var propertyName = property.Name;

                //Kiểm tra property có phải khóa chính không
                var primarykey = Attribute.IsDefined(property, typeof(Primarykey));

                //Thực hiện tạo Id mới cho khóa chính
                if (primarykey == true || propertyName == $"{className}ID")
                {
                    if(property.PropertyType == typeof(Guid))
                    {
                        propertyValue = Guid.NewGuid();
                    }
                }
                parameters.Add($"@{propertyName}", propertyValue);
            }
            int result = 0;
            //Khới tạo kết nối đến DB MySQL

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Thực hiện gọi vào DB
                result = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            //Xử lý kết quả trả về 
            return result;
        }

        /// <summary>
        /// Hàm sửa thông tin bản ghi theo id
        /// </summary>
        /// <param name="recordId">Id của bản ghi cần sửa</param>
        /// <returns>1: Nếu sửa thành công, 0: Nếu sưa thất bại</returns>
        public int UpdateRecord(T entity, Guid entityId)
        {
            //Lấy ra tên đối tượng entity
            var className = typeof(T).Name;

            //Chuẩn bị câu lệnh SQL
            var storedProcedureName = string.Format(Procedure.UPDATE_BY_ID, className);

            //Lấy danh sách tất cả thuộc tính của đối tượng
            var properties = typeof(T).GetProperties();

            //Chuẩn bị tham số đầu vào 
            var parameters = new DynamicParameters();

            //Lặp qua từng thuọc tính 
            foreach (var property in properties)
            {
                //Lấy ra giá trị của từng thuộc tính
                var propertyValue = property.GetValue(entity);
                var propertyName = property.Name;
                parameters.Add($"@{propertyName}", propertyValue);
                parameters.Add($"@{className}ID", entityId);
            }
            int result = 0;
            //Khới tạo kết nối đến DB MySQL

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Thực hiện gọi vào DB
                result = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            //Xử lý kết quả trả về 
            return result;
        }

        
    }
}
