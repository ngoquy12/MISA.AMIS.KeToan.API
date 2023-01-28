using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common.Resources;
using System.Security.Cryptography.X509Certificates;
using MISA.AMIS.KeToan.Common.Attributes;
using System.Reflection;

namespace MISA.AMIS.KeToan.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Xoá thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id của bản ghi muốn xóa</param>
        /// <returns>Id của bản ghi vừa xóa</returns>
        /// Created by: NVQUY(05/01/2023)
        public int DeleteRecordById(Guid recordId)
        {
            return _baseDL.DeleteRecordById(recordId);
        }

        /// <summary>
        /// Lấy thông tin tất cả  bản ghi 
        /// </summary>
        /// <returns>Danh sách bản ghi cần lấy</returns>
        /// Created by: NVQUY(05/01/2023)
        public IEnumerable<T> GetAllRecord()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Xoá thông tin một bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id của bản ghi muốn xóa</param>
        /// <returns>Id của bản ghi vừa xóa</returns>
        /// Created by: NVQUY(05/01/2023)
        public T GetRecordById(Guid recordId)
        {
            return _baseDL.GetRecordById(recordId);
        }

        /// <summary>
        /// Hàm xử lý dữ liệu nhập vào từ người dùng 
        /// Author: NVQUY(05/01/2023)
        /// </summary>
        /// <param name="employee">Dữ liệu trả vào từ người dùng</param>
        /// <returns>True: Nếu có dữ liệu nhập vào, False:Nếu không có dữ liệu nhập vào từ người dùng</returns>
        public static ServiceResponse ValidateRequestData(T record)
        {
            //Lấy danh sách tất cả thuộc tính của đối tượng
            var properties = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(RequiredAttribute)));
            var validateFailures = new List<string>();

            //Lặp qua từng thuọc tính 
            foreach (var property in properties)
            {
                //Lấy ra giá trị của từng thuộc tính
                var propertyValue = property.GetValue(record);
                var requiredAttribute = (RequiredAttribute?)property.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;
                if (requiredAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailures.Add(requiredAttribute.ErrorMessage);
                }
            }
            if (validateFailures.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = (int) StatusRespone.Failure,
                    Data = new ErrorService
                    {
                        ErrorCode = ErrorCode.InvaliteData,
                        UserMsg = String.Join(", ", validateFailures.ToArray()),
                        DevMsg = ResourceVN.DevMsg_InvalidData,
                        MoreInfo = ResourceVN.Error_MoreInfo,
                        Data = validateFailures,
                    }
                };
            }
            return new ServiceResponse
            {
                Success = (int) StatusRespone.Successful
            };
        }

        /// <summary>
        /// Hàm validate riêng của các đối tượng entity
        /// </summary>
        /// <param name="entity">Đối tượng validate riêng</param>
        protected virtual ServiceResponse ValidateCustom(T entity)
        {
            return new ServiceResponse
            {
                Success = (int) StatusRespone.Successful
            };
        }

        /// <summary>
        /// Cập nhật dữ liệu một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần cập nhật</param>
        /// <param name="entityId">Id của đối tượng cần cập nhật</param>
        /// <returns>1: Nếu cập nhật thành công, 0: Nếu cập nhật thất bại</returns>
        public ServiceResponse UpdateRecord(T entity, Guid entityId)
        {
            //Hàm validate dùng chung
            var validateResult = ValidateRequestData(entity);

            //Hàm validate dùng riêng
            var validateCustom = ValidateCustom(entity);

            //Validate dữ liệu
            if (validateResult.Success == (int)StatusRespone.Failure)
            {
                return new ServiceResponse
                {
                    Success = (int) StatusRespone.Failure,
                    Data = validateResult.Data
                };
            }
            else
            {
                if (validateCustom.Success == (int) StatusRespone.Successful)
                {
                    var result = _baseDL.UpdateRecord(entity, entityId);
                    if (result > 0)
                    {
                        return new ServiceResponse
                        {
                            Success = (int)StatusRespone.Successful,
                            Data = result
                        };
                    }
                }
                return new ServiceResponse
                {
                    Success = (int)StatusRespone.Failure,
                    Data = validateCustom.Data
                };
            }
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Dữ liệu từ người dùng</param>
        /// <returns>1: Nếu thêm thành công, 0: Nếu thêm mới thất bại</returns>
        public ServiceResponse InsertRecord(T entity)
        {
            //Hàm validate dùng chung
            var validateResult = ValidateRequestData(entity);

            //Hàm validate dùng riêng
            var validateCustom = ValidateCustom(entity);

            //Validate dữ liệu
            if (validateResult.Success == (int)StatusRespone.Failure)
            {
                return new ServiceResponse
                {
                    Success = (int)StatusRespone.Failure,
                    Data = validateResult.Data
                };
            }
            else
            {
                if (validateCustom.Success == (int)StatusRespone.Successful)
                {
                    var result = _baseDL.InsertRecord(entity);
                    if (result > 0)
                    {
                        return new ServiceResponse
                        {
                            Success = (int)StatusRespone.Successful,
                            Data = result
                        };
                    }
                }
                return new ServiceResponse
                {
                    Success = (int)StatusRespone.Failure,
                    Data = validateCustom.Data
                };
            }
        }

        /// <summary>
        /// Gắn giá trị cho từng thuộc tính riêng của từng đối tượng
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void BeforeInsert(ref T entity)
        {

        }

        public int DeleteMultiPath(ListRecordId listRecordId)
        {
            return _baseDL.DeleteMultiPath(listRecordId);
        }

        #endregion
    }
}
