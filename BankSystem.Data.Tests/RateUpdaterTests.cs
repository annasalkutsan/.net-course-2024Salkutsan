using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using Moq;
using Xunit;

public class RateUpdaterTests
{
    private readonly Mock<IClientStorage> _clientStorageMock;
    private readonly RateUpdater _rateUpdater;

    public RateUpdaterTests()
    {
        _clientStorageMock = new Mock<IClientStorage>();
        
        decimal interestRate = 0.05m; // 5%
        TimeSpan updateInterval = TimeSpan.FromHours(1); // 1 час
       
        _rateUpdater = new RateUpdater(_clientStorageMock.Object, interestRate, updateInterval);
    }
    [Fact]
    public async Task UpdateAccountsAsync()
    {
        // Arrange
        var currentTime = DateTime.UtcNow;
        var accounts = new List<Account>
        {
            new Account
            {
                Amount = 1000,
                LastUpdated = currentTime.AddDays(-31) 
            },
            new Account
            {
                Amount = 2000,
                LastUpdated = currentTime.AddDays(-29) 
            }
        };

        // для возврата списка аккаунтов
        _clientStorageMock.Setup(cs => cs.GetAllAccountsAsync()).ReturnsAsync(accounts);

        // для обновления аккаунтов
        _clientStorageMock.Setup(cs => cs.UpdateAccountAsync(It.IsAny<Account>()))
            .Returns(Task.CompletedTask);

        // Act
        await _rateUpdater.UpdateAccountsAsync();

        // Assert
        // обновление произошло для первого аккаунта
        Assert.Equal(1050, accounts[0].Amount); // 1000 + (1000 * 0.05)

        // LastUpdated для первого аккаунта установлен на время больше или равно currentTime
        Assert.True(accounts[0].LastUpdated >= currentTime, "LastUpdated должно быть больше или равно текущему времени");

        // второй аккаунт не был обновлен
        Assert.Equal(2000, accounts[1].Amount);
        Assert.Equal(currentTime.AddDays(-29), accounts[1].LastUpdated);
    }
}