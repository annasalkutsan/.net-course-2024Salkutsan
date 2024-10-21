using System.Globalization;
using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace ExportTool
{
    public class ExportService<T> where T : class
    {
        private readonly IStorage<T> _storage;
        private readonly BankSystemDbContext _context;

        public ExportService(IStorage<T> storage, BankSystemDbContext context)
        {
            _storage = storage;
            _context = context;
        }

        public void ExportToCsv( string pathToDirectory, string csvFileName)
        {
            var entities = _storage.GetAll();

            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, csvFileName);

            using (var writer = new StreamWriter(fullPath))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(entities);
                }
            }
        }
        
        public void ImportFromCsv(string pathToDirectory, string csvFileName)
        {
            string fullPath = Path.Combine(pathToDirectory, csvFileName);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("CSV файл не найден.");
            }

            using (var reader = new StreamReader(fullPath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(
                        new TypeConverterOptions { Formats = new[] { "dd.MM.yyyy HH:mm:ss" } }
                    );

                    var records = csv.GetRecords<T>().ToList();

                    foreach (var record in records)
                    {
                        //все свойства типа T
                        var properties = typeof(T).GetProperties();

                        foreach (var property in properties)
                        {
                            //является ли свойство DateTime
                            if (property.PropertyType == typeof(DateTime))
                            {
                                var dateTimeValue = (DateTime)property.GetValue(record);

                                //установка Kind на Utc, если он не установлен
                                if (dateTimeValue.Kind != DateTimeKind.Utc)
                                {
                                    dateTimeValue = DateTime.SpecifyKind(dateTimeValue, DateTimeKind.Utc);
                                    property.SetValue(record, dateTimeValue);
                                }
                            }
                        }

                        try
                        {
                            _storage.Add(record);
                            Console.WriteLine($"Запись успешно импортирована.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при импорте записи: {ex.Message}");
                        }
                    }
                    _context.SaveChanges();
                }
            }
        }
    }
}