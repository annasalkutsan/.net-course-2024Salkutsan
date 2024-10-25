using System.Globalization;
using System.Text.Json;
using BankSystem.App.Interfaces;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace ExportTool
{
    public class ExportService<T> where T : class
    {
        private readonly IStorage<T> _storage;
        private static readonly object _lock = new object();
        private const int MaxFileSize = 15 * 1024; // 15 kB

        public ExportService(IStorage<T> storage)
        {
            _storage = storage;
        }
        public ExportService() { }
        
        public void ExportToJson(string pathToDirectory, string jsonFileName)
        {
            var entities = _storage.GetAll();

            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, jsonFileName);
            string json = JsonSerializer.Serialize(entities);
            File.WriteAllText(fullPath, json);
        }
        
        public void ExportToJson(string pathToDirectory, string jsonFileName, ICollection<T> entities)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            int fileCounter = 1;
            string currentFileName = Path.Combine(pathToDirectory, $"{Path.GetFileNameWithoutExtension(jsonFileName)}_{fileCounter}.json");
            long currentFileSize = 0;

            lock (_lock) 
            {
                using (var enumerator = entities.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string jsonEntity = JsonSerializer.Serialize(enumerator.Current);
                        byte[] entityBytes = System.Text.Encoding.UTF8.GetBytes(jsonEntity + Environment.NewLine);
                    
                        // провека текущего размера файла перед записью
                        if (currentFileSize + entityBytes.Length > MaxFileSize)
                        {
                            // закрываем текущий файл и создаем новый
                            fileCounter++;
                            currentFileName = Path.Combine(pathToDirectory, $"{Path.GetFileNameWithoutExtension(jsonFileName)}_{fileCounter}.json");
                            currentFileSize = 0; // сбрасываем размер файла
                        }

                        using (FileStream fileStream = new FileStream(currentFileName, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            fileStream.Write(entityBytes, 0, entityBytes.Length);
                            currentFileSize += entityBytes.Length; // увеличиваем текущий размер файла
                        }
                    }
                }
            }
        }
        
        public void ExportToJson(string pathToDirectory, string jsonFileName, T entity)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, jsonFileName);
            string json = JsonSerializer.Serialize(entity);
            File.WriteAllText(fullPath, json);
        }
 
        public T ImportEntityFromJson(string pathToDirectory, string jsonFileName)
        {
            string fullPath = Path.Combine(pathToDirectory, jsonFileName);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("JSON файл не найден.");
            }

            string json = File.ReadAllText(fullPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            };
            
            var records = JsonSerializer.Deserialize<List<T>>(json, options);
            return records?.FirstOrDefault(); 
        }

        public ICollection<T> ImportCollectionFromJson(string pathToDirectory, string jsonFileName)
        {
            string fullPath = Path.Combine(pathToDirectory, jsonFileName);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("JSON файл не найден.");
            }

            string json = File.ReadAllText(fullPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var records = JsonSerializer.Deserialize<List<T>>(json, options);

            return records;
        }
        
        public void ImportFromJson(string pathToDirectory, string jsonFileName)
        {
            string fullPath = Path.Combine(pathToDirectory, jsonFileName);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("JSON файл не найден.");
            }

            string json = File.ReadAllText(fullPath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // для разных регистров свойств
            };

            var records = JsonSerializer.Deserialize<List<T>>(json, options);

            foreach (var record in records)
            {
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
                }
            }
        }
    }
}