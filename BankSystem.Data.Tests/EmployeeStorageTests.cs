using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Xunit;

namespace BankSystem.Data.Tests;

public class EmployeeStorageTests
{
    private readonly EmployeeStorage _employeeStorage;
    private readonly TestDataGenerator _dataGenerator;

    public EmployeeStorageTests()
    {
        _employeeStorage = new EmployeeStorage();
        _dataGenerator = new TestDataGenerator();
    }

    [Fact]
    public void AddEmployeePositivTest()
    {
        var employee = _dataGenerator.GenerateEmployees(1);
        
        _employeeStorage.AddEmployee(employee[0]);
        
        Assert.Single(_employeeStorage.GetAllEmployees());
    }

    [Fact]
    public void AddEmployeeCollectionPositivTest()
    {
        var employees = _dataGenerator.GenerateEmployees(5);

        _employeeStorage.AddEmployee(employees);

        Assert.Equal(5, _employeeStorage.GetAllEmployees().Count);
    }
    
    [Fact]
    public void GetYoungestEmployeePositivTest()
    {
        var employees = _dataGenerator.GenerateEmployees(10);
        _employeeStorage.AddEmployee(employees);

        var youngestEmployee = _employeeStorage.GetYoungestEmployee();
        
        Assert.NotNull(youngestEmployee);
        Assert.Equal(employees.OrderBy(c => c.BirthDay).First(), youngestEmployee);
    }

    [Fact]
    public void GetOldestEmployeePositivTest()
    {
        var employees = _dataGenerator.GenerateEmployees(10);
        _employeeStorage.AddEmployee(employees);

        var oldestEmployee = _employeeStorage.GetOldestEmployee();
        
        Assert.NotNull(oldestEmployee);
        Assert.Equal(employees.OrderByDescending(c => c.BirthDay).First(), oldestEmployee);
    }

    [Fact]
    public void GetAverageAgePositivTest()
    {
        var employees = _dataGenerator.GenerateEmployees(10);
        _employeeStorage.AddEmployee(employees);

        var averageAge = _employeeStorage.GetAverageAgeEmployee();
        var expectedAverageAge = employees
            .Select(e=>DateTime.Now.Year - e.BirthDay.Year - (DateTime.Now.DayOfYear < e.BirthDay.DayOfYear ? 1 : 0))
            .Average();
        
        Assert.Equal(expectedAverageAge, averageAge, 1);
    }
    
    [Fact]
    public void EditEmployee_PositiveTest()
    {
        var employee = _dataGenerator.GenerateEmployees(1)[0];
        _employeeStorage.AddEmployee(employee);

        var newEmployee = new Employee
        {
            FirstName = employee.FirstName,
            LastName = "Обновленный", 
            PhoneNumber = "1234567890",
            Position = "Менеджер", 
            BirthDay = employee.BirthDay, 
            Contract = employee.Contract,
            Salary = employee.Salary
        };

        _employeeStorage.EditEmployee(employee, newEmployee);
        
        var employees = _employeeStorage.GetAllEmployees();
        Assert.Single(employees); 
        Assert.Equal(newEmployee.LastName, employees[0].LastName); 
    }

    [Fact]
    public void EditEmployee_NullOldEmployee_ThrowsArgumentNullException()
    {
        var newEmployee = new Employee(); 
       
        Assert.Throws<ArgumentNullException>(() => _employeeStorage.EditEmployee(null, newEmployee));
    }
    
    [Fact]
    public void RemoveEmployees_PositiveTest()
    {
        var employees = _dataGenerator.GenerateEmployees(3);
        _employeeStorage.AddEmployee(employees);
        
        _employeeStorage.RemoveEmployees(employees);

        Assert.Empty(_employeeStorage.GetAllEmployees());
    }

    [Fact]
    public void RemoveEmployees_EmptyList_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _employeeStorage.RemoveEmployees(new List<Employee>()));
    }
}