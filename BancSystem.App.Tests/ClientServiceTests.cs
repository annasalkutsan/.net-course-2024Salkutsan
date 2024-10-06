using BankSystem.App.Exceptions;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Moq;
using Xunit;

namespace BancSystem.App.Tests;

public class ClientServiceTests
{
    private readonly Mock<ClientStorage> _clientStorageMock;
    private readonly ClientService _clientService;
    private readonly TestDataGenerator _dataGenerator;

    public ClientServiceTests()
    {
        _clientStorageMock = new Mock<ClientStorage>();
        _clientService = new ClientService(_clientStorageMock.Object);
        _dataGenerator = new TestDataGenerator();
    }

    [Fact]
    public void AddClientPositiveTest()
    {
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorageMock.Setup(storage => storage.AddClient(client));

        _clientService.AddClient(client);

        _clientStorageMock.Verify(storage => storage.AddClient(client), Times.Once);
    }

    [Fact]
    public void AddClientNegativeTestAgeException()
    {
        var client = new Client
        {
            BirthDay = DateTime.Now.AddYears(-17), 
            Passport = "AB123456"
        };

        Assert.Throws<AgeException>(() => _clientService.AddClient(client));
    }

    [Fact]
    public void AddClientNegativeTestPassportException()
    {
        var client = new Client
        {
            BirthDay = DateTime.Now.AddYears(-20), 
            Passport = null
        };

        Assert.Throws<PassportException>(() => _clientService.AddClient(client));
    }

    [Fact]
    public void AddClientWithAccountsPositiveTest()
    {
        var client = _dataGenerator.GenerateClients(1).First();
        var accounts = _dataGenerator.GenerateClientAccounts(new List<Client> { client })[client];

        _clientStorageMock.Setup(storage => storage.AddClient(client, accounts));

        _clientService.AddClient(client, accounts);

        _clientStorageMock.Verify(storage => storage.AddClient(client, accounts), Times.Once);
    }

    [Fact]
    public void AddClientsPositiveTest()
    {
        var clients = _dataGenerator.GenerateClients(5);
        var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);

        _clientStorageMock.Setup(storage => storage.AddClients(clientAccounts));

        _clientService.AddClients(clientAccounts);

        _clientStorageMock.Verify(storage => storage.AddClients(clientAccounts), Times.Once);
    }

    [Fact]
    public void AddClientsNegativeTestAgeException()
    {
        var clients = new Dictionary<Client, List<Account>>
        {
            { new Client { BirthDay = DateTime.Now.AddYears(-17), Passport = "AB123456" }, new List<Account>() }
        };

        Assert.Throws<AgeException>(() => _clientService.AddClients(clients));
    }

    [Fact]
    public void AddClientsNegativeTestPassportException()
    {
        var clients = new Dictionary<Client, List<Account>>
        {
            { new Client { BirthDay = DateTime.Now.AddYears(-20), Passport = null }, new List<Account>() }
        };

        Assert.Throws<PassportException>(() => _clientService.AddClients(clients));
    }

    [Fact]
    public void AddAccountToClientPositiveTest()
    {
        var client = _dataGenerator.GenerateClients(1).First();
        var account = new Account();

        _clientStorageMock.Setup(storage => storage.AddAccountToClient(client, account));

        _clientService.AddAccountToClient(client, account);

        _clientStorageMock.Verify(storage => storage.AddAccountToClient(client, account), Times.Once);
    }

    [Fact]
    public void AddAccountToClientNegativeTestNullAccount()
    {
        var client = _dataGenerator.GenerateClients(1).First();

        Assert.Throws<ArgumentNullException>(() => _clientService.AddAccountToClient(client, null));
    }

    [Fact]
    public void RemoveClientPositiveTest()
    {
        var client = _dataGenerator.GenerateClients(1).First();
        _clientStorageMock.Setup(storage => storage.RemoveClient(client));

        _clientService.RemoveClient(client);

        _clientStorageMock.Verify(storage => storage.RemoveClient(client), Times.Once);
    }

    [Fact]
    public void RemoveClientNegativeTestNullClient()
    {
        Assert.Throws<ArgumentNullException>(() => _clientService.RemoveClient(null));
    }

    [Fact]
    public void EditClientPositiveTest()
    {
        var oldClient = _dataGenerator.GenerateClients(1).First();
        var newClient = _dataGenerator.GenerateClients(1).First();

        _clientStorageMock.Setup(storage => storage.EditClient(oldClient, newClient));

        _clientService.EditClient(oldClient, newClient);

        _clientStorageMock.Verify(storage => storage.EditClient(oldClient, newClient), Times.Once);
    }

    [Fact]
    public void EditClientNegativeTestAgeException()
    {
        var newClient = new Client { BirthDay = DateTime.Now.AddYears(-17) };

        Assert.Throws<AgeException>(() => _clientService.EditClient(new Client(), newClient));
    }

    [Fact]
    public void EditClientNegativeTestPassportException()
    {
        var newClient = new Client { BirthDay = DateTime.Now.AddYears(-20), Passport = null };

        Assert.Throws<PassportException>(() => _clientService.EditClient(new Client(), newClient));
    }

    [Fact]
    public void EditAccountPositiveTest()
    {
        var client = _dataGenerator.GenerateClients(1).First();
        var oldAccount = new Account { Currency = new Currency("USD", "US Dollar"), Amount = 100 };
        var newAccount = new Account { Currency = new Currency("USD", "US Dollar"), Amount = 200 };

        _clientStorageMock.Setup(storage => storage.EditAccount(client, oldAccount, newAccount));

        _clientService.EditAccount(client, oldAccount, newAccount);

        _clientStorageMock.Verify(storage => storage.EditAccount(client, oldAccount, newAccount), Times.Once);
    }

    [Fact]
    public void GetClientsByFilterPositiveTest()
    {
        var clients = _dataGenerator.GenerateClients(5);
        var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);
    
        _clientStorageMock.Setup(storage => storage.GetClientsByFilter(null, null, null, null, null))
            .Returns(clientAccounts);

        var result = _clientService.GetClientsByFilter();

        Assert.Equal(5, result.Count);
    }


    [Fact]
    public void GetAllClientsPositiveTest()
    {
        var clients = _dataGenerator.GenerateClients(5);
        _clientStorageMock.Setup(storage => storage.GetAllClients())
            .Returns(clients);

        var result = _clientService.GetAllClients();

        Assert.Equal(5, result.Count);
    }
}