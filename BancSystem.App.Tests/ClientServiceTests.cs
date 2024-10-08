using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Xunit;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        private readonly IClientStorage _clientStorage;
        private readonly ClientService _clientService;
        private readonly TestDataGenerator _dataGenerator;

        public ClientServiceTests()
        {
            _clientStorage = new ClientStorage();
            _clientService = new ClientService(_clientStorage);
            _dataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void AddClientPositiveTest()
        {
            var client = _dataGenerator.GenerateClients(1).First();
            _clientService.Add(client);
            var allClients = _clientStorage.Get(c => true);
            Assert.Single(allClients);
            Assert.Equal(client, allClients.First().Key);
        }

        [Fact]
        public void AddClientNegativeTestAgeException()
        {
            var client = new Client
            {
                BirthDay = DateTime.Now.AddYears(-17),
                Passport = "AB123456"
            };
            Assert.Throws<AgeException>(() => _clientService.Add(client));
        }

        [Fact]
        public void AddClientNegativeTestPassportException()
        {
            var client = new Client
            {
                BirthDay = DateTime.Now.AddYears(-20),
                Passport = null
            };
            Assert.Throws<PassportException>(() => _clientService.Add(client));
        }

        [Fact]
        public void AddClientsPositiveTest()
        {
            var clients = _dataGenerator.GenerateClients(5);
            var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);
            _clientService.Add(clientAccounts);
            var allClients = _clientStorage.Get(c => true);
            Assert.Equal(5, allClients.Count);
        }

        [Fact]
        public void AddClientsNegativeTestAgeException()
        {
            var clients = new Dictionary<Client, List<Account>>
            {
                { new Client { BirthDay = DateTime.Now.AddYears(-17), Passport = "AB123456" }, new List<Account>() }
            };
            Assert.Throws<AgeException>(() => _clientService.Add(clients));
        }

        [Fact]
        public void AddClientsNegativeTestPassportException()
        {
            var clients = new Dictionary<Client, List<Account>>
            {
                { new Client { BirthDay = DateTime.Now.AddYears(-20), Passport = null }, new List<Account>() }
            };
            Assert.Throws<PassportException>(() => _clientService.Add(clients));
        }

        [Fact]
        public void AddAccountToClientPositiveTest()
        {
            var client = _dataGenerator.GenerateClients(1).First();
            var account = new Account();
            _clientService.Add(client);
            var clientAccounts = _clientStorage.Get(c => c == client).First().Value;
            Assert.Single(clientAccounts);
        }

        [Fact]
        public void AddAccountToClientNegativeTestNullAccount()
        {
            var client = _dataGenerator.GenerateClients(1).First();
            Assert.Throws<ArgumentNullException>(() => _clientService.AddAccount(client, null));
        }

        [Fact]
        public void RemoveClientPositiveTest()
        {
            var client = _dataGenerator.GenerateClients(1).First();
            _clientService.Add(client);
            _clientService.Delete(client);
            var allClients = _clientStorage.Get(c => true);
            Assert.Empty(allClients);
        }

        [Fact]
        public void RemoveClientNegativeTestNullClient()
        {
            Assert.Throws<ArgumentNullException>(() => _clientService.Delete(null));
        }

        [Fact]
        public void EditClientNegativeTestAgeException()
        {
            var newClient = new Client { BirthDay = DateTime.Now.AddYears(-17) };
            Assert.Throws<AgeException>(() => _clientService.Update(newClient));
        }

        [Fact]
        public void EditClientNegativeTestPassportException()
        {
            var newClient = new Client { BirthDay = DateTime.Now.AddYears(-20), Passport = null };
            Assert.Throws<PassportException>(() => _clientService.Update(newClient));
        }

        [Fact]
        public void GetClientsByFilterPositiveTest()
        {
            var clients = _dataGenerator.GenerateClients(5);
            foreach (var client in clients)
            {
                _clientService.Add(client);
            }
            var result = _clientService.GetClientsByFilter();
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void GetAllClientsPositiveTest()
        {
            var clients = _dataGenerator.GenerateClients(5);
            foreach (var client in clients)
            {
                _clientService.Add(client);
            }
            var result = _clientStorage.Get(c => true);
            Assert.Equal(5, result.Count);
        }
        
        
        [Fact]
        public void UpdateAccountPositiveTest()
        {
            var client = _dataGenerator.GenerateClients(1).First();
            _clientService.Add(client);
            
            var updatedAccount = new Account
            {
                Currency = new Currency("USD", "US Dollar"),
                Amount = 1500 
            };

            _clientService.UpdateAccount(client, updatedAccount);

            var clientAccounts = _clientStorage.Get(c => c == client).First().Value;
            
            Assert.Equal(updatedAccount.Currency.Code, clientAccounts.First().Currency.Code);
        }
        
        [Fact]
        public void DeleteAccountPositiveTest()
        {
            var client = _dataGenerator.GenerateClients(1).First();
            var account = new Account();
            _clientService.Add(client);

            _clientService.AddAccount(client, account);

            _clientService.DeleteAccount(client, account);

            var clientAccountsAfterDelete = _clientStorage.Get(c => c == client).First().Value;
            Assert.Single(clientAccountsAfterDelete);
        }


    }
}
