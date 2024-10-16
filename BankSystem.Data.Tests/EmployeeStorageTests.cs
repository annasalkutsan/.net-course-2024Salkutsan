using BankSystem.App.Services;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Data.Storages;
using Xunit;

public class EmployeeStorageTests
{
    private readonly BankSystemDbContext _context;
    private readonly EmployeeStorage _employeeStorage;
    private readonly TestDataGenerator _dataGenerator;

    public EmployeeStorageTests()
    {
        _context = new BankSystemDbContext();
        _employeeStorage = new EmployeeStorage(_context);
        _dataGenerator = new TestDataGenerator();
    }

    [Fact]
    public void GetEmployeeById()
    {
        // Arrange
        var employee = _dataGenerator.GenerateEmployees(1).First();
        _employeeStorage.Add(employee);

        // Act
        var retrievedEmployee = _employeeStorage.Get(employee.Id);

        // Assert
        Assert.NotNull(retrievedEmployee);
        Assert.Equal(employee, retrievedEmployee);
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

        // Act
        var retrievedEmployees = _employeeStorage.GetAll();

        // Assert
        Assert.Equal(employees.Count, retrievedEmployees.Count);
    }

    [Fact]
    public void AddEmployee()
    {
        // Arrange
        var employee = _dataGenerator.GenerateEmployees(1).First();

        // Act
        _employeeStorage.Add(employee);
        var addedEmployee = _context.Employees.Find(employee.Id);

        // Assert
        Assert.NotNull(addedEmployee);
        Assert.Equal(employee.FirstName, addedEmployee.FirstName);
        Assert.Equal(employee.LastName, addedEmployee.LastName);
    }

    [Fact]
    public void UpdateEmployee()
    {
        // Arrange
        var employee = _dataGenerator.GenerateEmployees(1).First();
        _employeeStorage.Add(employee);
        employee.FirstName = "UpdatedName";

        // Act
        _employeeStorage.Update(employee.Id, employee);

        // Assert
        var updatedEmployee = _context.Employees.Find(employee.Id);
        Assert.Equal("UpdatedName", updatedEmployee.FirstName);
    }

    [Fact]
    public void DeleteEmployee()
    {
        // Arrange
        var employee = _dataGenerator.GenerateEmployees(1).First();
        _employeeStorage.Add(employee);

        // Act
        _employeeStorage.Delete(employee.Id);

        // Assert
        var deletedEmployee = _employeeStorage.Get(employee.Id);
        Assert.Null(deletedEmployee);
    }

    [Fact]
    public void GetByFilterEmployees()
    {
        // Arrange
        var employees = _dataGenerator.GenerateEmployees(5);
        foreach (var employee in employees)
        {
            _employeeStorage.Add(employee);
        }

        // Act
        var filteredEmployees = _employeeStorage.GetByFilter(e => e.FirstName.StartsWith("А"));

        // Assert
        Assert.NotEmpty(filteredEmployees);
    }
}
