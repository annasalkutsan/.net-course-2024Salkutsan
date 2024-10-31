using BankSystem.App.Services;
using Xunit;

namespace BancSystem.App.Tests
{
    public class WithdrawFromAccountsTests
    {
        private List<Account> _accounts;
        private TestDataGenerator _dataGenerator;

        public WithdrawFromAccountsTests()
        {
            _dataGenerator = new TestDataGenerator();
            _accounts = _dataGenerator.GenerateAccounts(3);
        }

        private async Task<bool[]> WithdrawFromAccountsAsync(decimal amount)
        {
            List<Task<bool>> withdrawalTasks = new List<Task<bool>>();

            foreach (var account in _accounts)
            {
                withdrawalTasks.Add(Task.Run(() => account.Withdraw(amount)));
            }

            return await Task.WhenAll(withdrawalTasks);
        }
        
        [Fact]
        public async Task WithdrawFromAccountsAsyncTest()
        {
            // Arrange
            decimal amountToWithdraw = 20;

            // Act
            var results = await WithdrawFromAccountsAsync(amountToWithdraw);

            // Assert
            Assert.Equal(_accounts.Count, results.Length); 
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i]) //было ли списание успешным
                {
                    Assert.Equal(80m, _accounts[i].Amount); //ожидаем, что сумма уменьшилась до 80 (изначально = 100)
                }
                else
                {
                    Assert.Equal(100m, _accounts[i].Amount);
                }
            }
        }
    }
}