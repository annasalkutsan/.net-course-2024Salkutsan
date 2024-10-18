using System.Globalization;
using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Domain.Models;
using CsvHelper;


namespace ExportTool
{
    public class ExportService
    {
        private readonly IClientStorage _clientStorage;
        private readonly BankSystemDbContext _context;
        public string _pathToDirectory { get; set; }
        public string _csvFileName { get; set; }

        public ExportService(IClientStorage clientStorage, string pathToDirectory, string csvFileName)
        {
            _context = new BankSystemDbContext();
            _clientStorage = clientStorage;
            _pathToDirectory = pathToDirectory;
            _csvFileName = csvFileName;
        }

        public void ExportClientsToCsv()
        {
            var clients = _clientStorage.GetAll();

            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);

            using (var writer = new StreamWriter(fullPath))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Client>();
                    csv.NextRecord();

                    foreach (var client in clients)
                    {
                        csv.WriteRecord(client);
                        csv.NextRecord();
                    }
                }
            }
        }
        
        public void ImportClientsFromCsv()
        {
            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("CSV файл не найден.");
            }

            using (var reader = new StreamReader(fullPath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<dynamic>().ToList();

                    foreach (var record in records)
                    {
                        try
                        {
                            string passport = record.Passport;
                            Guid id = Guid.Parse(record.Id);
                            string firstName = record.FirstName;
                            string lastName = record.LastName;
                            string phoneNumber = record.PhoneNumber;

                            DateTime birthDay = DateTime.ParseExact(record.BirthDay, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                                                   .ToUniversalTime(); 
                            DateTime createUtc = DateTime.ParseExact(record.CreateUtc, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                                                    .ToUniversalTime(); 

                            var client = new Client
                            {
                                Passport = passport,
                                Id = id,
                                FirstName = firstName,
                                LastName = lastName,
                                PhoneNumber = phoneNumber,
                                BirthDay = birthDay,
                                CreateUtc = createUtc
                            };

                            _context.Clients.Add(client);
                            Console.WriteLine($"Клиент с ID {client.Id} добавлен успешно.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при добавлении клиента с ID {record.Id}: {ex.Message}");
                        }
                    }
                    _context.SaveChanges();
                }
            }
        }
    }
}