using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Xunit;

namespace BancSystem.App.Tests;

public class EquivalenceTests
{
    /*[Fact]
    public void GetHashCodeNecessityPositivTest()
    {
        var testDataGenerator = new TestDataGenerator();
        var clients = testDataGenerator.GenerateClients(5);
        var clientAccounts = testDataGenerator.GenerateClientAccounts(clients);
        
        var existingClient = clients.First();
        var newClient = new Client(
            existingClient.Passport,
            existingClient.RegistrationDate,
            existingClient.FirstName,
            existingClient.LastName,
            existingClient.PhoneNumber,
            existingClient.BirthDay
        );
        
        Assert.False(clientAccounts.ContainsKey(newClient), "Ожидается, что клиент не найден в словаре без переопределения Equals и GetHashCode");
    }
    
    [Fact]
    public void GetHashCodeNecessityPositivTest_AfterOverride()
    {
        var testDataGenerator = new TestDataGenerator();
        var clients = testDataGenerator.GenerateClients(5);
        var clientAccounts = testDataGenerator.GenerateClientAccounts(clients);
        
        var existingClient = clients.First();
        var newClient = new Client(
            existingClient.Passport,
            existingClient.RegistrationDate,
            existingClient.FirstName,
            existingClient.LastName,
            existingClient.PhoneNumber,
            existingClient.BirthDay
        );
        
        Assert.True(clientAccounts.ContainsKey(newClient), "Ожидается, что клиент найден в словаре после переопределения Equals и GetHashCode");
    }
    
    [Fact]
    public void GenerateClientWithMultipleAccountsPositiveTest()
    {
        var testDataGenerator = new TestDataGenerator();
        var clients = testDataGenerator.GenerateClients(5);
        var clientAccounts = testDataGenerator.GenerateClientAccounts(clients);
    
        var existingClient = clients.First();

        var newClient = new Client(
            existingClient.Passport,
            existingClient.RegistrationDate,
            existingClient.FirstName,
            existingClient.LastName,
            existingClient.PhoneNumber,
            existingClient.BirthDay
        );

        Assert.True(clientAccounts.ContainsKey(newClient), "Ожидается, что клиент найден в словаре после переопределения Equals и GetHashCode с несколькими банковскими счетами");
    }
    
    [Fact]
    public void EmployeeExistsInListPositiveTest()
    {
        var testDataGenerator = new TestDataGenerator();
        var employees = testDataGenerator.GenerateEmployees(5);
    
        var existingEmployee = employees.First();

        var newEmployee = new Employee
        {
            FirstName = existingEmployee.FirstName,
            LastName = existingEmployee.LastName,
            PhoneNumber = existingEmployee.PhoneNumber,
            Position = existingEmployee.Position,
            BirthDay = existingEmployee.BirthDay,
            Contract = existingEmployee.Contract,
            Salary = existingEmployee.Salary
        };

        var employeeList = employees.ToList();

        Assert.Contains(newEmployee, employeeList);
    }*/
}