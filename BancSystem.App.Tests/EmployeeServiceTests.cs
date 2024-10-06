using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Xunit;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _employeeService;
        private readonly TestDataGenerator _dataGenerator;

        public EmployeeServiceTests()
        {
            _employeeService = new EmployeeService();
            _dataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void AddEmployeePositiveTest()
        {
            var employee = _dataGenerator.GenerateEmployees(1).First();
            _employeeService.AddEmployee(employee);

            var employees = _employeeService.GetAllEmployees();
            Assert.Single(employees);
            Assert.Equal(employee.FirstName, employees[0].FirstName);
            Assert.Equal(employee.LastName, employees[0].LastName);
        }

        [Fact]
        public void AddEmployeesPositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(3);
            _employeeService.AddEmployees(employees);

            var allEmployees = _employeeService.GetAllEmployees();
            Assert.Equal(3, allEmployees.Count);
        }

        [Fact]
        public void GetEmployeesByFilterReturnsFilteredEmployeesPositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(5);
            _employeeService.AddEmployees(employees);

            var filteredEmployees = _employeeService.GetEmployeesByFilter(lastName: employees[0].LastName);
            Assert.Single(filteredEmployees);
            Assert.Equal(employees[0].LastName, filteredEmployees[0].LastName);
        }

        [Fact]
        public void EditEmployeePositiveTest()
        {
            var oldEmployee = _dataGenerator.GenerateEmployees(1).First();
            _employeeService.AddEmployee(oldEmployee);

            var newEmployee = new Employee
            {
                FirstName = "Обновленный",
                LastName = "Сотрудник",
                PhoneNumber = oldEmployee.PhoneNumber,
                Position = oldEmployee.Position,
                BirthDay = oldEmployee.BirthDay
            };

            _employeeService.EditEmployee(oldEmployee, newEmployee);

            var employees = _employeeService.GetAllEmployees();
            Assert.Single(employees);
            Assert.Equal(newEmployee.FirstName, employees[0].FirstName);
        }

        [Fact]
        public void RemoveEmployeePositiveTest()
        {
            var employee = _dataGenerator.GenerateEmployees(1).First();
            _employeeService.AddEmployee(employee);
            _employeeService.RemoveEmployee(employee);

            var employees = _employeeService.GetAllEmployees();
            Assert.Empty(employees);
        }

        [Fact]
        public void RemoveEmployeesPositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(3);
            _employeeService.AddEmployees(employees);
            _employeeService.RemoveEmployees(employees);

            var allEmployees = _employeeService.GetAllEmployees();
            Assert.Empty(allEmployees);
        }
    }
}
