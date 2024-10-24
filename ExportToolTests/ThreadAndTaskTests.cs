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
        public void ExportClients()
        {
            var clients1 = _dataGenerator.GenerateClients(100);
            var clients2 = _dataGenerator.GenerateClients(100);

            Thread thread1 = new Thread(() => _exportService.ExportToJson(_testDirectory, _jsonFileName, clients1));
            Thread thread2 = new Thread(() => _exportService.ExportToJson(_testDirectory, _jsonFileName, clients2));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            var files = Directory.GetFiles(_testDirectory, "*.json");
            Assert.True(files.Length > 0, "Файлы не были созданы.");

            // размер файла меньше заданного максимума
            const long MaxFileSize = 32 * 1024;
            foreach (var file in files)
            {
                long fileSize = new FileInfo(file).Length;
                Assert.True(fileSize <= MaxFileSize,
                    $"Размер файла {Path.GetFileName(file)} превышает допустимый размер.");
            }
        }
        
        [Fact]
        public void ExportSingleClient()
        {
            var client1 = _dataGenerator.GenerateClients(1).First(); // Генерируем одного клиента
            var client2 = _dataGenerator.GenerateClients(1).First(); // Генерируем второго клиента

            Thread thread1 = new Thread(() => _exportService.ExportToJson(_testDirectory, _jsonFileName, new List<Client> { client1 }));
            Thread thread2 = new Thread(() => _exportService.ExportToJson(_testDirectory, _jsonFileName, new List<Client> { client2 }));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            var files = Directory.GetFiles(_testDirectory, "*.json");
            Assert.True(files.Length > 0, "Файлы не были созданы.");

            const long MaxFileSize = 32 * 1024;
            foreach (var file in files)
            {
                long fileSize = new FileInfo(file).Length;
                Assert.True(fileSize <= MaxFileSize,
                    $"Размер файла {Path.GetFileName(file)} превышает допустимый размер.");
            }
        }

        [Fact]
        public void AccountReplenishment()
        {
            var account = _dataGenerator.GenerateAccount();

            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (account) 
                    {
                        account.AccountReplenishment(100);
                    }
                }
            });

            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (account) 
                    {
                        account.AccountReplenishment(100);
                    }
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
