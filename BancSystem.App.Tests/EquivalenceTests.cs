using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Xunit;

namespace BancSystem.App.Tests;

public class EquivalenceTests
{
    [Fact]
    public void GetHashCodeNecessityPositivTest()
    {
        /*
        * в классе “EquivalenceTests”, добавить метод
        “GetHashCodeNecessityPositivTest” в рамках которого получить
        словарь тестовых сущностей применяя метод описанный во втором
        пункте. 
        Создать нового клиента, значения свойств которого,
        соответствуют значениям одного из клиентов содержащихся в
        словаре. Попробовать получить счет клиента применив созданного
        клиента в качестве ключа;
         */
        
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
        
        Assert.False(clientAccounts.ContainsKey(newClient), "Клиент не найден в словаре без переопределения Equals и GetHashCode");
    }
    /*
     * переопределить методы Equals и GetHashCode, и повторить
        попытку. Работу оформить в виде теста, с проверкой соответствия
        результата ожиданию;
        ● усложнить задачу на случай, когда у клиента несколько банковских
        счетов;
        ● Реализовать аналогичный тест для списка - List<Emloyee>.
     */
    
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
        
        Assert.True(clientAccounts.ContainsKey(newClient), "Клиент найден в словаре после переопределения Equals и GetHashCode");
        var account = clientAccounts[newClient];
        Assert.NotNull(account);
    }
}