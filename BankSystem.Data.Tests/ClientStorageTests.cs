using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Xunit;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
   private readonly ClientStorage _clientStorage;
        private readonly TestDataGenerator _testDataGenerator;

        public ClientStorageTests()
        {
            _clientStorage = new ClientStorage();
            _testDataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void AddClient_PositiveTest()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];

            // Act
            _clientStorage.Add(client);

            // Assert
            var clients = _clientStorage.GetAllClients();
            Assert.Contains(client, clients);
        }

        [Fact]
        public void AddClient_NegativeTest_ClientAlreadyExists()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];
            _clientStorage.Add(client);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _clientStorage.Add(client));
            Assert.Equal("Клиент уже существует в хранилище.", exception.Message);
        }

        [Fact]
        public void UpdateClient_PositiveTest()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];
            _clientStorage.Add(client);
            var updatedClient = new Client(client.Passport, client.RegistrationDate, "UpdatedFirstName", "UpdatedLastName", client.PhoneNumber, client.BirthDay);

            // Act
            _clientStorage.Update(updatedClient);

            // Assert
            var clients = _clientStorage.GetAllClients();
            Assert.Contains(updatedClient, clients);
        }

        [Fact]
        public void UpdateClient_NegativeTest_ClientNotFound()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => _clientStorage.Update(client));
            Assert.Equal("Клиент с указанным паспортом не найден.", exception.Message);
        }

        [Fact]
        public void DeleteClient_PositiveTest()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];
            _clientStorage.Add(client);

            // Act
            _clientStorage.Delete(client);

            // Assert
            var clients = _clientStorage.GetAllClients();
            Assert.DoesNotContain(client, clients);
        }

        /*[Fact]
        public void GetYoungestClient_PositiveTest()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(5);
            foreach (var client in clients)
            {
                _clientStorage.Add(client);
            }

            // Act
            var youngestClient = _clientStorage.GetYoungestClient();

            // Assert
            Assert.Equal(clients.OrderBy(c => c.BirthDay).Last().Passport, youngestClient.Passport);
        }

        [Fact]
        public void GetOldestClient_PositiveTest()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(5);
            foreach (var client in clients)
            {
                _clientStorage.Add(client);
            }

            // Act
            var oldestClient = _clientStorage.GetOldestClient();

            // Assert
            Assert.Equal(clients.OrderBy(c => c.BirthDay).First().Passport, oldestClient.Passport);
        }*/

        [Fact]
        public void GetAverageAgeClient_PositiveTest()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(5);
            foreach (var client in clients)
            {
                _clientStorage.Add(client);
            }

            // Act
            var averageAge = _clientStorage.GetAverageAgeClient();

            // Assert
            var expectedAverageAge = clients.Average(c => DateTime.Now.Year - c.BirthDay.Year - (DateTime.Now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0));
            Assert.Equal(expectedAverageAge, averageAge, 1); // Погрешность 1 год
        }

        [Fact]
        public void AddDictionary_PositiveTest()
        {
            // Arrange
            var clients = _testDataGenerator.GenerateClients(3);
            var clientAccounts = _testDataGenerator.GenerateClientAccounts(clients);

            // Act
            _clientStorage.AddDictionary(clientAccounts);

            // Assert
            foreach (var client in clients)
            {
                Assert.Contains(client, _clientStorage.GetAllClients());
            }
        }

        [Fact]
        public void AddAccount_PositiveTest()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];
            _clientStorage.Add(client);
            var account = new Account(new Currency("EUR", "Евро"), 1000);

            // Act
            _clientStorage.AddAccount(client, account);

            // Assert
            var accounts = _clientStorage.GetClientAccounts(client);
            Assert.Contains(account, accounts);
        }

        [Fact]
        public void UpdateAccount_PositiveTest()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];
            _clientStorage.Add(client);
            var originalAccount = new Account(new Currency("USD", "Доллар США"), 1000);
            _clientStorage.AddAccount(client, originalAccount);
            var updatedAccount = new Account(new Currency("USD", "Доллар США"), 2000);

            // Act
            _clientStorage.UpdateAccount(client, updatedAccount);

            // Assert
            var accounts = _clientStorage.GetClientAccounts(client);
            Assert.Contains(updatedAccount, accounts); // Проверка, что обновленный аккаунт присутствует
        }
        
        [Fact]
        public void DeleteAccount_PositiveTest()
        {
            // Arrange
            var client = _testDataGenerator.GenerateClients(1)[0];
            _clientStorage.Add(client);
    
            var account1 = new Account(new Currency("EUR", "Евро"), 2000);
            
            _clientStorage.AddAccount(client, account1);

            // Act
            _clientStorage.DeleteAccount(client, account1);

            // Assert
            var accounts = _clientStorage.GetClientAccounts(client);
            
            Assert.Single(accounts); ;
        }

    }