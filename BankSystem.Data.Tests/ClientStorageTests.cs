using BankSystem.App.Services;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Data.Storages;
using Xunit;

public class ClientStorageTests
{
    private readonly BankSystemDbContext _context;
    private readonly ClientStorage _clientStorage;
    private readonly TestDataGenerator _dataGenerator;

    public ClientStorageTests()
    {
        _context = new BankSystemDbContext();
        _clientStorage = new ClientStorage(_context);
        _dataGenerator = new TestDataGenerator();
    }

    [Fact]
    public void GetClientById()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorage.Add(client);

        // Act
        var retrievedClient = _clientStorage.Get(client.Id);

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
        var retrievedClients = _clientStorage.GetAll();

        // Assert
        Assert.Equal(clients.Count, retrievedClients.Count);
    }

    [Fact]
    public void AddClientAndDefaultAccount()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();

        // Act
        _clientStorage.Add(client);
        var addedClient = _context.Clients.Find(client.Id);
        var clientAccount = _context.Accounts.FirstOrDefault(a => a.ClientId == client.Id);

        // Assert
        Assert.NotNull(addedClient); 
        Assert.Equal(client.Id, addedClient.Id);
        Assert.NotNull(clientAccount); 
        Assert.Equal("USD", clientAccount.Currency.Code);
        Assert.Equal(0, clientAccount.Amount); // Проверяем, что баланс аккаунта 0
    }
    

    [Fact]
    public void UpdateClient()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorage.Add(client);
        client.FirstName = "UpdatedName";

        // Act
        _clientStorage.Update(client);

        // Assert
        var updatedClient = _context.Clients.Find(client.Id);
        Assert.Equal("UpdatedName", updatedClient.FirstName);
    }
    

    [Fact]
    public void DeletClient()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorage.Add(client);

        // Act
        _clientStorage.Delete(client);

        // Assert
        var deletedClient = _clientStorage.Get(client.Id);
        Assert.Null(deletedClient);
    }

    [Fact]
    public void GetByFilterClients()
    {
        // Arrange
        var clients = _dataGenerator.GenerateClients(5);
        foreach (var client in clients)
        {
            _clientStorage.Add(client);
        }

        // Act
        var filteredClients = _clientStorage.GetByFilter(c => c.FirstName.StartsWith("A"));

        // Assert
        Assert.NotEmpty(filteredClients);
    }

    [Fact]
    public void GetAccountsByClientId()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorage.Add(client);

        var newAccount = new Account(new Currency("EUR", "Евро"), 150) { ClientId = client.Id };
        _clientStorage.AddAccount(client.Id, newAccount); 

        // Act
        var accounts = _clientStorage.GetAccountsByClientId(client.Id);

        // Assert
        Assert.Equal(2, accounts.Count); 
    }
    
    [Fact]
    public void AddAccountToClient()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorage.Add(client);

        var newAccount = new Account(new Currency("EUR", "Евро"), 200); // Новый счет в EUR

        // Act
        _clientStorage.AddAccount(client.Id, newAccount); 

        // Assert
        var accounts = _clientStorage.GetAccountsByClientId(client.Id);
        var euroAccount = accounts.FirstOrDefault(a => a.Currency.Code == "EUR");
        Assert.NotNull(euroAccount);
        Assert.Equal(200, euroAccount.Amount); 
        Assert.Equal(client.Id, euroAccount.ClientId);
    }

    [Fact]
    public void UpdateAccount()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorage.Add(client); 

        var defaultAccount = _context.Accounts.First(); 
        var updatedAccount = new Account(defaultAccount.Currency, 500) 
        {
            Id = defaultAccount.Id // Используем тот же ID
        };

        // Act
        _clientStorage.UpdateAccount(updatedAccount); 

        // Assert
        var accountAfterUpdate = _context.Accounts.First(a => a.Id == defaultAccount.Id);
        Assert.Equal(500, accountAfterUpdate.Amount); 
    }
    
    [Fact]
    public void DeleteAccount()
    {
        // Arrange
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorage.Add(client);
        var newAccount = new Account(new Currency("EUR", "Евро"), 200); // Новый счет в EUR
        _clientStorage.AddAccount(client.Id, newAccount);

        // Act
        _clientStorage.DeleteAccount(newAccount.Id);

        // Assert
        var deletedAccount = _context.Accounts.Find(newAccount.Id);
        Assert.Null(deletedAccount);
    }
}
