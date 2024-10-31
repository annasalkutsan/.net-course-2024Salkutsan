using BankSystem.App.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BankSystem.App.Services
{
    public class RateUpdater : BackgroundService
    {
        private readonly IClientStorage _clientStorage;
        private readonly decimal _interestRate; // процентная ставка
        private readonly TimeSpan _updateInterval; // интервал обновления

        public RateUpdater(IClientStorage clientStorage, decimal interestRate, TimeSpan updateInterval)
        {
            _clientStorage = clientStorage;
            _interestRate = interestRate;
            _updateInterval = updateInterval;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateAccountsAsync();
                await Task.Delay(_updateInterval, stoppingToken);
            }
        }

        public async Task UpdateAccountsAsync()
        {
            var accounts = await _clientStorage.GetAllAccountsAsync();
            var currentTime = DateTime.UtcNow; 

            foreach (var account in accounts)
            {
                if ((currentTime - account.LastUpdated).TotalDays >= 30)
                {
                    var interest = account.Amount * _interestRate;
                    account.AccountReplenishment(interest);

                    account.LastUpdated = currentTime;

                    await _clientStorage.UpdateAccountAsync(account);
                }
            }
        }
    }
}