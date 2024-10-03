using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Xunit;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    private readonly ClientStorage _clientStorage;
    private readonly TestDataGenerator _dataGenerator;

    public ClientStorageTests()
    {
        _clientStorage = new ClientStorage();
        _dataGenerator = new TestDataGenerator();
    }

    [Fact]
    public void AddClientPositivTest()
    {
        var client = _dataGenerator.GenerateClients(1).First();
        var accounts = _dataGenerator.GenerateClientAccounts(new List<Client> { client })[client];

        _clientStorage.AddClient(client, accounts);

        Assert.Single(_clientStorage.GetAllClients());
    }
    
    [Fact]
    public void AddClientCollectionPositivTest()
    {
        var clients = _dataGenerator.GenerateClients(5);
        var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);

        _clientStorage.AddClients(clientAccounts);

        Assert.Equal(5, _clientStorage.GetAllClients().Count);
    }

    [Fact]
    public void GetYoungestClientPositivTest()
    {
        var clients = _dataGenerator.GenerateClients(10);
        var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);
        _clientStorage.AddClients(clientAccounts);
        
        var youngestClient = _clientStorage.GetYoungestClient();

        Assert.NotNull(youngestClient);
        Assert.Equal(clients.OrderBy(c => c.BirthDay).FirstOrDefault(), youngestClient);
    }

    [Fact]
    public void GetOldestClientPositivTest()
    {
        var clients = _dataGenerator.GenerateClients(10);
        var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);
        _clientStorage.AddClients(clientAccounts);

        var oldestClient = _clientStorage.GetOldestClient();

        Assert.NotNull(oldestClient);
        Assert.Equal(clients.OrderByDescending(c => c.BirthDay).FirstOrDefault(), oldestClient);
    }
    
    [Fact]
    public void GetAverageAgePositivTest()
    {
        var clients = _dataGenerator.GenerateClients(10);
        var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);
        _clientStorage.AddClients(clientAccounts);
        
        var averageAge = _clientStorage.GetAverageAgeClient();
        var expectedAverageAge = clients
            .Select(c => DateTime.Now.Year - c.BirthDay.Year - (DateTime.Now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0))
            .Average();

        Assert.Equal(expectedAverageAge, averageAge, 1);
    }
    
    [Fact]
    public void GetAllClientsPositivTest()
    {
        var clients = _dataGenerator.GenerateClients(5);
        var clientAccounts = _dataGenerator.GenerateClientAccounts(clients);
        _clientStorage.AddClients(clientAccounts);

        var allClients = _clientStorage.GetAllClients();

        Assert.Equal(5, allClients.Count);
        foreach (var client in clients)
        {
            Assert.Contains(client, allClients);
        }
    }

    [Fact]
    public void GetClientAccountsPositivTest()
    {
        var client = _dataGenerator.GenerateClients(1).First();
        var accounts = _dataGenerator.GenerateClientAccounts(new List<Client> { client })[client];

        _clientStorage.AddClient(client, accounts);

        var clientAccounts = _clientStorage.GetClientAccounts(client);

        Assert.NotNull(clientAccounts);
        Assert.Equal(accounts.Count, clientAccounts.Count);
        Assert.Equal(accounts, clientAccounts);
    }

    [Fact]
    public void GetClientAccountsReturnsEmptyListForUnknownClient()
    {
        var unknownClient = new Client { Passport = "UNKNOWN" };
        var clientAccounts = _clientStorage.GetClientAccounts(unknownClient);

        Assert.NotNull(clientAccounts);
        Assert.Empty(clientAccounts);
    }

}