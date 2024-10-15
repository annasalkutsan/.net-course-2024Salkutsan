using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Data.Storages;
using Xunit;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        private readonly IEmployeeStorage _employeeStorage;
        private readonly EmployeeService _employeeService;
        private readonly TestDataGenerator _dataGenerator;
        private BankSystemDbContext _context;

        public EmployeeServiceTests()
        {
            _context = new BankSystemDbContext();
            _employeeStorage = new EmployeeStorage(_context);
            _employeeService = new EmployeeService(_employeeStorage);
            _dataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void GetEmployee()
        {
            // Arrange
            var employee = _dataGenerator.GenerateEmployees(1).First();
            _employeeStorage.Add(employee);
            _context.SaveChanges(); 

            // Act
            var retrievedEmployee = _employeeService.GetEmployee(employee.Id);

            // Assert
            Assert.NotNull(retrievedEmployee);
            Assert.Equal(employee.LastName, retrievedEmployee.LastName);
        }

        [Fact]
        public void GetAllEmployees()
        {
            // Arrange
            var employees = _dataGenerator.GenerateEmployees(3);
            foreach (var employee in employees)
            {
                _employeeStorage.Add(employee);
            }
            _context.SaveChanges(); 

            // Act
            var retrievedEmployees = _employeeService.GetAllEmployees();

            // Assert
            Assert.Equal(employees.Count, retrievedEmployees.Count);
        }

        [Fact]
        public void AddEmployee()
        {
            // Arrange
            var employee = _dataGenerator.GenerateEmployees(1).First();

            // Act
            _employeeService.AddEmployee(employee);
            _context.SaveChanges(); 

            // Assert
            var retrievedEmployee = _employeeService.GetEmployee(employee.Id);
            Assert.Equal(employee.LastName, retrievedEmployee.LastName);
        }

        [Fact]
        public void UpdateEmployee()
        {
            // Arrange
            var employee = _dataGenerator.GenerateEmployees(1).First();
            _employeeStorage.Add(employee);
            _context.SaveChanges(); 
            
            // Act
            employee.LastName = "Обновленный"; 
            _employeeService.UpdateEmployee(employee);
            _context.SaveChanges(); 
            
            // Assert
            var retrievedEmployee = _employeeService.GetEmployee(employee.Id);
            Assert.Equal("Обновленный", retrievedEmployee.LastName);
        }

        [Fact]
        public void DeleteEmployee()
        {
            // Arrange
            var employee = _dataGenerator.GenerateEmployees(1).First();
            _employeeStorage.Add(employee);
            _context.SaveChanges(); 
            
            // Act
            _employeeService.DeleteEmployee(employee);
            _context.SaveChanges(); 

            // Assert
            Assert.Throws<KeyNotFoundException>(() => _employeeService.GetEmployee(employee.Id));
        }

        [Fact]
        public void GetEmployeesByFilter()
        {
            // Arrange
            var employees = _dataGenerator.GenerateEmployees(5);
            foreach (var employee in employees)
            {
                _employeeStorage.Add(employee);
            }
            _context.SaveChanges(); 

            // Act
            var filteredEmployees = _employeeService.GetEmployeesByFilter(lastName: employees[0].LastName);

            // Assert
            Assert.Single(filteredEmployees);
            Assert.Equal(employees[0].LastName, filteredEmployees.First().LastName);
        }
    }
}
