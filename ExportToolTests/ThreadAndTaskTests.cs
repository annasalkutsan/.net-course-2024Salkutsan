using System.Text.Json;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using ExportTool;
using Xunit;

namespace ExportToolTests
{
    public class ThreadAndTaskTests
    {
        private readonly string _testDirectory = @"D:\Dex\Practic\.net-course-2024Salkutsan\ExportToolTests\TestJson";
        private readonly string _jsonFileName = "test_clients_thread.json";

        private readonly TestDataGenerator _dataGenerator;
        private readonly ExportService<Client> _exportService;

        public ThreadAndTaskTests()
        {
            _dataGenerator = new TestDataGenerator();
            _exportService = new ExportService<Client>();
        }

        [Fact]
        public void ExportToJsonClients()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
            Directory.CreateDirectory(_testDirectory);

            var clients1 = _dataGenerator.GenerateClients(50);
            var clients2 = _dataGenerator.GenerateClients(100);

            Thread thread1 = new Thread(() => _exportService.ExportToJson(_testDirectory, _jsonFileName, clients1));
            Thread thread2 = new Thread(() => _exportService.ExportToJson(_testDirectory, _jsonFileName, clients2));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            var files = Directory.GetFiles(_testDirectory, $"{Path.GetFileNameWithoutExtension(_jsonFileName)}_*.json");
            Assert.Equal(3, files.Length);

            int expectedTotalClients = clients1.Count + clients2.Count;
            int actualTotalClients = 0;

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file); //каждая строка как отдельный JSON объект
                actualTotalClients += lines.Length;

                //проверка десериализации каждой строки как объекта Client
                foreach (var line in lines)
                {
                    var client = JsonSerializer.Deserialize<Client>(line);

                    Assert.NotNull(client);
                    Assert.True(!string.IsNullOrEmpty(client.FirstName) && !string.IsNullOrEmpty(client.Passport), "Клиент должен содержать как минимум FirstName и Passport");
                }
            }

            //проверка общего количества клиентов
            Assert.Equal(expectedTotalClients, actualTotalClients);
        }
        
        [Fact]
        public void AccountReplenishment()
        {
            var account = _dataGenerator.GenerateAccount();

            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                   account.AccountReplenishment(100);
                }
            });

            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                { 
                    account.AccountReplenishment(100);
                }
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Assert.Equal(2000, account.Amount); // 2000$
        }
    }
}
    

