using BankSystem.Domain.Models;
using Bogus;

namespace BankSystem.App.Services
{
    public class TestDataGenerator
    {
        public List<Client> GenerateClients(int count)
        {
            var clientFaker = new Faker<Client>("ru")
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.PhoneNumber, f => $"+373 777 55 {f.Random.Number(100, 999)}")
                .RuleFor(c => c.Passport, f => $"{f.Random.AlphaNumeric(2).ToUpper()}{f.Random.Number(100000, 999999)}")
                .RuleFor(e => e.BirthDay, f => f.Date.Past(100, DateTime.UtcNow.AddYears(-18)));

            return clientFaker.Generate(count);
        }

        public List<Employee> GenerateEmployees(int count)
        {
            var employeeFaker = new Faker<Employee>("ru")
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.PhoneNumber, f => $"+373 777 66 {f.Random.Number(100, 999)}")
                .RuleFor(e => e.Position, f => new Position(f.Name.JobTitle())) // Генерация должности
                .RuleFor(e => e.BirthDay, f => f.Date.Past(19))
                .RuleFor(e => e.Contract, f => "Контракт для сотрудника")
                .RuleFor(e => e.Salary, f => f.Random.Number(1000, 50000));

            return employeeFaker.Generate(count);
        }

        public List<Position> GeneratePositions(int count)
        {
            var positionFaker = new Faker<Position>("ru")
                .RuleFor(p => p.Title, f => f.Name.JobTitle());

            return positionFaker.Generate(count);
        }

        public List<Currency> GenerateCurrencies(int count)
        {
            var currencyFaker = new Faker<Currency>()
                .RuleFor(c => c.Code, f => f.Finance.Currency().Code)
                .RuleFor(c => c.Name, f => f.Finance.Currency().Description);

            return currencyFaker.Generate(count);
        }

        public List<Account> GenerateAccounts(int count, List<Client> clients)
        {
            var accountFaker = new Faker<Account>()
                .RuleFor(a => a.Currency, f => new Currency(f.Finance.Currency().Code, f.Finance.Currency().Description))
                .RuleFor(a => a.Amount, f => f.Finance.Amount(100, 10000))
                .RuleFor(a => a.ClientId, f => f.PickRandom(clients).Id);

            return accountFaker.Generate(count);
        }
    }
}
