using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.EntityConfigurations;
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
        private BankSystemDbContext _context;

        public ClientServiceTests()
        {
            _context = new BankSystemDbContext(); 
            _clientStorage = new ClientStorage(_context);
            _clientService = new ClientService(_clientStorage);
            _dataGenerator = new TestDataGenerator();
        }
        
        [Fact]
        public void GetClient()
        {
            // Arrange
            var client = _dataGenerator.GenerateClients(1).First();
            _clientStorage.Add(client);

            // Act
            var retrievedClient = _clientService.GetClient(client.Id);

            // Assert
            Assert.NotNull(retrievedClient);
            Assert.Equal(client, retrievedClient);
        }
        
        [Fact]
        public void GetAllClients()
        {
            // Arrange
            var clients = _dataGenerator.GenerateClients(3);
            foreach (var client in clients)
            {
                _clientStorage.Add(client); 
            }

            // Act
            var retrievedClients = _clientService.GetAllClients();

            // Assert
            Assert.Equal(clients.Count, retrievedClients.Count);
        }

        [Fact]
        public void AddClient()
        {
            // Arrange
            var client = _dataGenerator.GenerateClients(1).First(); 
            // Act
            _clientService.AddClient(client); 

            // Assert
            var retrievedClient = _clientStorage.Get(client.Id); 
            Assert.NotNull(retrievedClient);
            Assert.Equal(client, retrievedClient); 
        }
        
        [Fact]
        public void UpdateClient()
        {
            // Arrange
            var originalClient = _dataGenerator.GenerateClients(1).First(); 
            _clientStorage.Add(originalClient); 

            var updatedClient = new Client
            {
                Id = originalClient.Id,
                FirstName = "Обновленное имя",
                LastName = originalClient.LastName,
                Passport = originalClient.Passport,
                PhoneNumber = originalClient.PhoneNumber,
                BirthDay = originalClient.BirthDay
            };

            // Act
            _clientService.UpdateClient(updatedClient.Id, updatedClient); 

            // Assert
            var retrievedClient = _clientStorage.Get(originalClient.Id);
            Assert.NotNull(retrievedClient);
            Assert.Equal("Обновленное имя", retrievedClient.FirstName); 
        }
        
        [Fact]
        public void GetClientsByFilter()
        {
            // Arrange
            var client1 = new Client { FirstName = "Bvz", LastName = "Иванов", PhoneNumber = "1234567890", Passport = "AB123456", BirthDay = new DateTime(1990, 1, 1) };
            var client2 = new Client { FirstName = "Bvz", LastName = "Петров", PhoneNumber = "0987654321", Passport = "CD987654", BirthDay = new DateTime(1985, 5, 15) };
            var client3 = new Client { FirstName = "Bvz", LastName = "Сидоров", PhoneNumber = "1122334455", Passport = "EF123456", BirthDay = new DateTime(1995, 10, 10) };
            
            _clientStorage.Add(client1);
            _clientStorage.Add(client2);
            _clientStorage.Add(client3);

            // Act
            var filteredClients = _clientService.GetClientsByFilter(lastName: "Петров"); 

            // Assert
            Assert.Equal("Петров", filteredClients.First().LastName); 
        }
        [Fact]
        public void GetAccountsByClientId()
        {
            var clients = _dataGenerator.GenerateClients(1); 
            var accounts = _dataGenerator.GenerateAccounts(2, clients); 

            // Добавляем клиента и его аккаунты в хранилище
            _clientStorage.Add(clients.First());
            foreach (var account in accounts)
            {
                _clientService.AddAccount(clients.First().Id, account);
            }

            // Act
            var retrievedAccounts = _clientService.GetAccountsByClientId(clients.First().Id); 

            // Assert
            Assert.Equal(3, retrievedAccounts.Count);
        }
        
        [Fact]
        public void AddAccount()
        {
            // Arrange
            var clients = _dataGenerator.GenerateClients(1); 
            _clientStorage.Add(clients.First()); 
            var account = new Account 
            {
                Id = Guid.NewGuid(),
                Currency = new Currency("MLD", "Молдавский Лей"),
                Amount = 5000,
                ClientId = clients.First().Id 
            };

            // Act
            _clientService.AddAccount(clients.First().Id, account); 
            
            // Assert
            var retrievedAccounts = _clientService.GetAccountsByClientId(clients.First().Id); 
            Assert.NotEqual(account.Amount, retrievedAccounts.First().Amount); 
        }
        
        [Fact]
        public void UpdateAccount()
        {
            // Arrange
            var clients = _dataGenerator.GenerateClients(1);
            _clientStorage.Add(clients.First());
            var account = _clientStorage.GetAccountsByClientId(clients.First().Id);
            account.First().Amount = 2000; 

            // Act
            _clientService.UpdateAccount(account.First()); 

            // Assert
            var updatedAccount = _clientService.GetAccountsByClientId(clients.First().Id).FirstOrDefault(a => a.Id == account.First().Id);
            Assert.Equal(account.First().Amount, updatedAccount.Amount);
        }
        
        [Fact]
        public void DeleteAccount()
        {
            // Arrange
            var clients = _dataGenerator.GenerateClients(1); 
            _clientStorage.Add(clients.First()); 

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Currency = new Currency("USD", "Доллар США"),
                Amount = 1000,
                ClientId = clients.First().Id 
            };

            _clientStorage.AddAccount(clients.First().Id, account); 
            
            // Act
            _clientService.DeleteAccount(account.Id); 

            // Assert
            var deletedAccount = _clientService.GetAccountsByClientId(clients.First().Id).FirstOrDefault(a => a.Id == account.Id);
            Assert.Null(deletedAccount);
        }
    }
}
