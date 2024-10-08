using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using BankSystem.Data.Storages;
using Xunit;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        private readonly IEmployeeStorage _employeeStorage;
        private readonly EmployeeService _employeeService;
        private readonly TestDataGenerator _dataGenerator;

        public EmployeeServiceTests()
        {
            _employeeStorage = new EmployeeStorage();
            _employeeService = new EmployeeService(_employeeStorage);
            _dataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void AddEmployeePositiveTest()
        {
            var employee = _dataGenerator.GenerateEmployees(1).First();
            _employeeService.Add(employee);
            var employees = _employeeService.GetEmployeesByFilter();
            Assert.Single(employees);
            Assert.Equal(employee.FirstName, employees[0].FirstName);
            Assert.Equal(employee.LastName, employees[0].LastName);
        }

        [Fact]
        public void AddEmployeesPositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(3);
            _employeeService.AddCollection(employees);
            var allEmployees = _employeeService.GetEmployeesByFilter();
            Assert.Equal(3, allEmployees.Count);
        }

        [Fact]
        public void GetEmployeesByFilterReturnsFilteredEmployeesPositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(5);
            _employeeService.AddCollection(employees);
            var filteredEmployees = _employeeService.GetEmployeesByFilter(lastName: employees[0].LastName);
            Assert.Single(filteredEmployees);
            Assert.Equal(employees[0].LastName, filteredEmployees[0].LastName);
        }

        [Fact]
        public void EditEmployeePositiveTest()
        {
            var oldEmployee = _dataGenerator.GenerateEmployees(1).First();
            _employeeService.Add(oldEmployee);
            var newEmployee = new Employee
            {
                FirstName = "Обновленный",
                LastName = "Сотрудник",
                PhoneNumber = oldEmployee.PhoneNumber,
                Position = oldEmployee.Position,
                BirthDay = oldEmployee.BirthDay
            };
            _employeeService.Update(newEmployee);
            var employees = _employeeService.GetEmployeesByFilter();
            Assert.Single(employees);
            Assert.Equal(newEmployee.FirstName, employees[0].FirstName);
        }

        [Fact]
        public void RemoveEmployeePositiveTest()
        {
            var employee = _dataGenerator.GenerateEmployees(1).First();
            _employeeService.Add(employee);
            _employeeService.Delete(employee);
            var employees = _employeeService.GetEmployeesByFilter();
            Assert.Empty(employees);
        }

        [Fact]
        public void RemoveEmployeesPositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(3);
            _employeeService.AddCollection(employees);
            foreach (var employee in employees)
            {
                _employeeService.Delete(employee);
            }
            var allEmployees = _employeeService.GetEmployeesByFilter();
            Assert.Empty(allEmployees);
        }
    }
}
