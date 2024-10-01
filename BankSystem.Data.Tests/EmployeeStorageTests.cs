using BankSystem.App.Services;
using BankSystem.Data.Storages;
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
        
        employees.ForEach(employee=>_employeeStorage.AddEmployee(employee));

        var youngestEmployee = _employeeStorage.GetYoungestEmployee();
        
        Assert.NotNull(youngestEmployee);
        Assert.Equal(employees.OrderBy(c => c.BirthDay).First(), youngestEmployee);
    }

    [Fact]
    public void GetOldestEmployeePositivTest()
    {
        var employees = _dataGenerator.GenerateEmployees(10);
        employees.ForEach(employee=>_employeeStorage.AddEmployee(employee));

        var oldestEmployee = _employeeStorage.GetOldestEmployee();
        
        Assert.NotNull(oldestEmployee);
        Assert.Equal(employees.OrderByDescending(c => c.BirthDay).First(), oldestEmployee);
    }

    [Fact]
    public void GetAverageAgePositivTest()
    {
        var employees = _dataGenerator.GenerateEmployees(10);
        employees.ForEach(employee=>_employeeStorage.AddEmployee(employee));

        var averageAge = _employeeStorage.GetAverageAgeEmployee();
        var expectedAverageAge = employees
            .Select(e=>DateTime.Now.Year - e.BirthDay.Year - (DateTime.Now.DayOfYear < e.BirthDay.DayOfYear ? 1 : 0))
            .Average();
        
        Assert.Equal(expectedAverageAge, averageAge, 1);
    }
}