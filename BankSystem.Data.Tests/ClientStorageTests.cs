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
        var client = _dataGenerator.GenerateClients(1);

        _clientStorage.AddClient(client[0]);

        Assert.Single(_clientStorage.GetAllClients());
    }

    [Fact]
    public void GetYoungestClientPositivTest()
    {
        var clients = _dataGenerator.GenerateClients(10);
        clients.ForEach(client=>_clientStorage.AddClient(client));
        
        var youngestClient = _clientStorage.GetYoungestClient();

        Assert.NotNull(youngestClient);
        Assert.Equal(clients.OrderBy(c => c.BirthDay).FirstOrDefault(), youngestClient);
    }

    [Fact]
    public void GetOldestClientPositivTest()
    {
        var clients = _dataGenerator.GenerateClients(10);
        clients.ForEach(client=>_clientStorage.AddClient(client));

        var oldestClient = _clientStorage.GetOldestClient();

        Assert.NotNull(oldestClient);
        Assert.Equal(clients.OrderByDescending(c => c.BirthDay).FirstOrDefault(), oldestClient);
    }
    
    [Fact]
    public void GetAverageAgePositivTest()
    {
        var clients = _dataGenerator.GenerateClients(10);
        clients.ForEach(client=>_clientStorage.AddClient(client));
        
        var averageAge = _clientStorage.GetAverageAgeClient();
        var expectedAverageAge = clients
            .Select(c => DateTime.Now.Year - c.BirthDay.Year - (DateTime.Now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0))
            .Average();

        Assert.Equal(expectedAverageAge, averageAge, 1);
    }
}