using MISA.AMIS.KeToan.API.Controllers;
using MISA.AMIS.KeToan.API.Entyties;

namespace MISA.AMIS.KeToan.API.UnitTests
{
    public class EmployeeControllerTest
    {
        /// <summary>
        /// Hàm test dữ liệu đầu vào mã nhân viên để trống
        /// Author: NVQUY(04/01/2023)
        /// </summary>
        [Test]
        public void InsertEmployee_ValidInputEmpoyeeCode_ReturnsError400BadRequest()
        {
            //Arrange - Chuẩn bị tất cả tham số đầu vào
            Employee employee = new Employee();
            employee.EmployeeCode = "";
            employee.EmployeeName = "Ngo Van Quy";
            employee.Email = "nvquy@gmail.com";
            employee.DepartmentID = new Guid("11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");
            var expectedResult = 400 ;

            var insertEmployee = new EmployeeUnitTestsController();

            //Act - Gọi hàm cần test
            var actualResult = insertEmployee.InsertEmployee(employee);

            //Assert - Kiểm tra kết quả có đúng mong đợi không

            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Hàm test dữ liệu đầu vào tên nhân viên để trống
        /// Author: NVQUY(04/01/2023)
        /// </summary>
        [Test]
        public void InsertEmployee_ValidInputEmpoyeeName_ReturnsError400BadRequest()
        {
            //Arrange - Chuẩn bị tất cả tham số đầu vào
            Employee employee = new Employee();
            employee.EmployeeCode = "NV009";
            employee.EmployeeName = "";
            employee.Email = "nvquy@gmail.com";
            employee.DepartmentID = new Guid("11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");
            var expectedResult = 400;

            var insertEmployee = new EmployeeUnitTestsController();

            //Act - Gọi hàm cần test
            var actualResult = insertEmployee.InsertEmployee(employee);

            //Assert - Kiểm tra kết quả có đúng mong đợi không

            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Hàm test dữ liệu đầu vào email không đúng định dạng
        /// Author: NVQUY(04/01/2023)
        /// </summary>
        [Test]
        public void InsertEmployee_InvalidEmail_ReturnsError400BadRequest()
        {
            //Arrange - Chuẩn bị tất cả tham số đầu vào
            Employee employee = new Employee();
            employee.EmployeeCode = "NV009";
            employee.EmployeeName = "Ngo Van Quy";
            employee.Email = "nvquy";
            employee.DepartmentID = new Guid("11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");
            var expectedResult = 400;

            var insertEmployee = new EmployeeUnitTestsController();

            //Act - Gọi hàm cần test
            var actualResult = insertEmployee.InsertEmployee(employee);

            //Assert - Kiểm tra kết quả có đúng mong đợi không

            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Hàm test dữ liệu đầu vào trùng mã nhân viên
        /// Author: NVQUY(04/01/2023)
        /// </summary>
        [Test]
        public void InsertEmployee_DuplicateInputEmployeeCode_ReturnsError400BadRequest()
        {
            //Arrange - Chuẩn bị tất cả tham số đầu vào
            Employee employee = new Employee();
            employee.EmployeeCode = "NV808849";
            employee.EmployeeName = "Ngo Van Quy";
            employee.Email = "nvquy@gmail.com";
            employee.DepartmentID = new Guid("11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");
            var expectedResult = 400;

            var insertEmployee = new EmployeeUnitTestsController();

            //Act - Gọi hàm cần test
            var actualResult = insertEmployee.InsertEmployee(employee);

            //Assert - Kiểm tra kết quả có đúng mong đợi không

            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Hàm test đúng dữ liệu đầu vào
        /// Author: NVQUY(04/01/2023)
        /// </summary>
        [Test]
        public void InsertEmployee_ValidInput_ReturnsError201Created()
        {
            //Arrange - Chuẩn bị tất cả tham số đầu vào
            Employee employee = new Employee();
            employee.EmployeeCode = "NV8088";
            employee.EmployeeName = "Ngo Van Quy";
            employee.Email = "nvquy@gmail.com";
            employee.DepartmentID = new Guid("11452b0c-768e-5ff7-0d63-eeb1d8ed8cef");
            var expectedResult = 201;

            var insertEmployee = new EmployeeUnitTestsController();

            //Act - Gọi hàm cần test
            var actualResult = insertEmployee.InsertEmployee(employee);

            //Assert - Kiểm tra kết quả có đúng mong đợi không

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
