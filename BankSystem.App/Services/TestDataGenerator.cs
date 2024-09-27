using BankSystem.Domain.Models;
using Bogus;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    
    public List<Client> GenerateClients(int count)
    {
        var clientFaker = new Faker<Client>("ru")
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.PhoneNumber, f => $"+373 777 55 {f.Random.Number(100, 999)}")
            .RuleFor(c => c.Passport, f => $"{f.Random.AlphaNumeric(2).ToUpper()}{f.Random.Number(100000, 999999)}")
            .RuleFor(c => c.RegistrationDate, f => f.Date.Past(10))
            .RuleFor(c => c.BirthDay, f => f.Date.Past(18));
        return clientFaker.Generate(count);
    }

    public Dictionary<string, Client> GenerateClientDictionary(List<Client> clients)
    {
        return clients.GroupBy(client => client.PhoneNumber)
            .ToDictionary(group => group.Key, group => group.First());
    }

    public List<Employee> GenerateEmployees(int count)
    {
       var employeeFaker = new Faker<Employee>("ru")
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.PhoneNumber, f => $"+373 777 66 {f.Random.Number(100, 999)}")
            .RuleFor(e => e.Position, f => "Сотрудник")
            .RuleFor(e => e.BirthDay, f => f.Date.Past(18))
            .RuleFor(e => e.Contract, f => "Контракт для сотрудника")
            .RuleFor(e => e.Salary, f => f.Random.Number(1000, 50000));
        return employeeFaker.Generate(count);
    }
    
    public Dictionary<Client, List<Account>> GenerateClientAccounts(List<Client> clients)
    {
        var accountFaker = new Faker<Account>()
            .RuleFor(a => a.Currency, f => new Currency(f.Finance.Currency().Code, f.Finance.Currency().Description))
            .RuleFor(a => a.Amount, f => f.Finance.Amount(100, 10000));

        var clientAccounts = new Dictionary<Client, List<Account>>();

        foreach (var client in clients)
        {
            int accountCount = new Random().Next(1, 4);
            var accounts = accountFaker.Generate(accountCount);

            clientAccounts[client] = accounts; 
        }
        
        return clientAccounts;
    }

}