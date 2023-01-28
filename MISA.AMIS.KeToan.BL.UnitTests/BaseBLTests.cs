using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.DL;
using NSubstitute;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL.UnitTests
{
    public class BaseBLTests
    {
        /// <summary>
        /// Hàm test lấy thông tin một nhân viên theo Id
        /// </summary>
        [Test]
        public void GetRecordById_EmployeeIdExits_ReturnsNull()
        {
            //Arrage
            var recordId = Guid.NewGuid();

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            var expectedResult = new Employee
            {
                EmployeeID = recordId,
                EmployeeCode = "NV009",
                EmployeeName = "Ngọ Văn Quý",
                Gender = 1,
            };

            fakeBaseDL.GetRecordById(recordId).Returns(expectedResult);

            var baseBl = new BaseBL<Employee>(fakeBaseDL);

            //Act
            var actualResult = baseBl.GetRecordById(recordId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Hàm test trường hợp không có Id
        /// </summary>
        [Test]
        public void GetRecordById_EmployeeIdNotExits_ReturnsNull()
        {
            //Arrage
            var recordId = Guid.NewGuid();

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();
            fakeBaseDL.GetRecordById(recordId).Returns((Employee)null);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            //Act
            var actualResult = baseBL.GetRecordById(recordId);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Test trường hợp các dữ liệu nhập vào đã hợp lệ
        /// </summary>
        [Test]
        public void InsertRecord_Employee_ReturnsSuccessful()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var employee = new Employee()
            {
                EmployeeCode = "NV827482",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                Gender = 1,
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(1);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Successful;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp mã nhân viên để trống
        /// </summary>
        [Test]
        public void InsertRecord_EmployeeCodeEmpty_ReturnsFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var employee = new Employee()
            {
                EmployeeCode = "",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Assert - So sánh kết quả thực tế và kết quả mong đợi
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp tên nhân viên để trống
        /// </summary>
        [Test]
        public void InsertRecord_EmployeeNameEmpty_ReturnsFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var employee = new Employee()
            {
                EmployeeCode = "NV0007",
                EmployeeName = "",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Assert - So sánh kết quả thực tế và kết quả mong đợi
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp tên nhân viên để trống
        /// </summary>
        [Test]
        public void InsertRecord_DepartmentIdEmpty_ReturnsFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var employee = new Employee()
            {
                EmployeeCode = "NV0007",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = Guid.Empty,
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Assert - So sánh kết quả thực tế và kết quả mong đợi
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp emai không đúng định dạng
        /// </summary>
        [Test]
        public void InsertRecord_EmailNotInvalid_ReturnFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong đợi
            var employee = new Employee()
            {
                EmployeeCode = "NV0823",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                Email = "nvquy"
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Thực hiện gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Act - So sánh kết quả thực tế và kết quả mong đợi
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp ngày sinh lớn hơn ngày hiện tại
        /// </summary>
        [Test]
        public void InsertRecord_DateOfBirthBiggerDateNow_ReturnFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong đợi
            DateTime dateNow = DateTime.Now;
            var dateFuture = dateNow.AddDays(10);

            var employee = new Employee()
            {
                EmployeeCode = "NV827482",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                Gender = 1,
                DateOfBirth = dateFuture
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Thực hiện gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Act - So sánh kết quả thực tế và kết quả mong đợi
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp ngày cấp chứng minh nhân dân lớn hơn ngày hiện tại
        /// </summary>
        [Test]
        public void InsertRecord_IssuaDateBiggerDateNow_ReturnFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong đợi
            DateTime dateNow = DateTime.Now;
            var dateFuture = dateNow.AddDays(10);

            var employee = new Employee()
            {
                EmployeeCode = "NV827482",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                Gender = 1,
                IdentityIssueDate = dateFuture
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Thực hiện gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Act - So sánh kết quả thực tế và kết quả mong đợi
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp mã nhân viên phải kết thúc bằng số
        /// </summary>
        [Test]
        public void InsertRecord_EmployeeCodeEndNumber_ReturnsFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var employee = new Employee()
            {
                EmployeeCode = "Nv098ss",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                Gender = 1,
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Assert - So sánh kết quả thực tế với kết quả mong muốn
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Test trường hợp mã nhân viên vượt quá độ dài
        /// </summary>
        [Test]
        public void InsertRecord_EmployeeCodeMaxLength_ReturnFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var employee = new Employee()
            {
                EmployeeCode = "NV0987777777777777777777777777777777777777777777",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                Gender = 1,
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Assert - So sánh kết quả thực tế với kết quả mong muốn
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        [Test]
        public void InsertRecord_DuplicateEmployeeCode_ReturnFailed()
        {
            //Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var employee = new Employee()
            {
                EmployeeCode = "NV097642",
                EmployeeName = "Ngọ Văn Quý",
                DepartmentID = new Guid("469b3ece-744a-45d5-957d-e8c757976496"),
                Gender = 1,
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();

            fakeBaseDL.InsertRecord(employee).Returns(0);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            var expectedResult = (int)StatusRespone.Failure;

            //Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(employee);

            //Assert - So sánh kết quả thực tế với kết quả mong muốn
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }
    }
}
