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
                // Создаем задачу для списания средств с каждого счета
                withdrawalTasks.Add(Task.Run(() => account.Withdraw(amount)));
            }

            // Ждем завершения всех задач
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
                if (results[i]) // Проверяем, было ли списание успешным
                {
                    Assert.Equal(80m, _accounts[i].Amount); // Ожидаем, что сумма уменьшилась до 80
                }
                else
                {
                    Assert.Equal(100m, _accounts[i].Amount); // Ожидаем, что сумма осталась без изменений
                }
            }
        }
    }
}