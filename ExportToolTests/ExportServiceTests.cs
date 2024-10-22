using System.Text.Json;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using ExportTool;
using Xunit;

namespace ExportToolTests;

public class ExportServiceTests
{
    private readonly string _testDirectory = @"D:\Dex\Practic\.net-course-2024Salkutsan\ExportToolTests\TestCsv";
    private readonly string _csvFileName = "test_clients.csv";
    private readonly string _jsonFileName = "test_clients.json";
    
    private readonly IClientStorage _clientStorage;
    private readonly TestDataGenerator _dataGenerator;
    private readonly BankSystemDbContext _context;
    private readonly ExportService<Client> _exportService; 

    public ExportServiceTests()
    {
        _context = new BankSystemDbContext();
        _clientStorage = new ClientStorage(_context);
        _dataGenerator = new TestDataGenerator();
        
        _exportService = new ExportService<Client>(_clientStorage);
    }

    [Fact]
    public void ExportToJsonPositiveTest()
    {
        // Arrange
        var entities = _dataGenerator.GenerateClients(5); 
        foreach (var entity in entities)
        {
            _context.Add(entity); 
            _context.SaveChanges();
        }

        // Act
        _exportService.ExportToJson(_testDirectory, _jsonFileName); 

        // Assert
        string fullPath = Path.Combine(_testDirectory, _jsonFileName);
        Assert.True(File.Exists(fullPath)); 

        var json = File.ReadAllText(fullPath);
        var deserializedEntities = JsonSerializer.Deserialize<List<Client>>(json);

        Assert.Equal(entities.Count, deserializedEntities.Count); 
    }
    
    [Fact]
    public void ImportFromJsonPositiveTest()
    {
        // Arrange
        var entities = _dataGenerator.GenerateClients(5); 
        string json = JsonSerializer.Serialize(entities); 

        // Создание тестового JSON-файла
        string fullPath = Path.Combine(_testDirectory, _jsonFileName);
        Directory.CreateDirectory(_testDirectory); 
        File.WriteAllText(fullPath, json);

        // Act
        _exportService.ImportFromJson(_testDirectory, _jsonFileName); 
        _context.SaveChanges(); 
        
        // Assert
        var importedEntities = _clientStorage.GetAll(); 
        Assert.Equal(entities.Count, importedEntities.Count); 
    }
    
    [Fact]
    public void ExportClientsToCsvPositiveTest()
    {
        // Arrange
        var clients = _dataGenerator.GenerateClients(5); 
        foreach (var client in clients)
        {
            _clientStorage.Add(client);
            _context.SaveChanges(); 
        }

        // Act
        _exportService.ExportToCsv(_testDirectory, _csvFileName); 
        
        // Assert
        string fullPath = Path.Combine(_testDirectory, _csvFileName);
        Assert.True(File.Exists(fullPath)); 

        var lines = File.ReadAllLines(fullPath);
        Assert.Equal(6, lines.Length); // 1 заголовок + 5 записей клиентов
        
        Assert.Contains("Passport,Id,FirstName,LastName,PhoneNumber,BirthDay,CreateUtc", lines[0]);
    }

    [Fact]
    public void ImportClientsFromCsvPositiveTest()
    {
        // Arrange
        var clients = _dataGenerator.GenerateClients(3); 
        string fullPath = Path.Combine(_testDirectory, _csvFileName);

        using (var writer = new StreamWriter(fullPath))
        {
            writer.WriteLine("Passport,Id,FirstName,LastName,PhoneNumber,BirthDay,CreateUtc");
            foreach (var client in clients)
            {
                writer.WriteLine($"{client.Passport},{client.Id},{client.FirstName},{client.LastName},{client.PhoneNumber},{client.BirthDay},{client.CreateUtc}");
            }
        }

        // Act
        _exportService.ImportFromCsv(_testDirectory, _csvFileName); 
        _context.SaveChanges(); 

        // Assert
        var importedClients = _clientStorage.GetAll();
        Assert.Equal(3, importedClients.Count); 

        foreach (var client in clients)
        {
            Assert.Contains(importedClients, c => c.Passport == client.Passport); 
        }
    }
}