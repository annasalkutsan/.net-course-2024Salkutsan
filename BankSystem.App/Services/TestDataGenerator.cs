﻿using BankSystem.Domain.Models;
using Bogus;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    
    public List<Client> GenerateClients(int count)
    {
        var clientFaker = new Faker<Client>("ru")
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.PhoneNumber, f => $"+373 777 55 {f.Random.Number(100, 999)}")
            .RuleFor(c => c.Passport, f => $"{f.Random.AlphaNumeric(2).ToUpper()}{f.Random.Number(100000, 999999)}")
            .RuleFor(c => c.RegistrationDate, f => f.Date.Past(10))
            .RuleFor(c => c.BirthDay, f => f.Date.Past(18));
        return clientFaker.Generate(count);
    }

    public Dictionary<string, Client> GenerateClientDictionary(List<Client> clients)
    {
        var clientDictionary = new Dictionary<string, Client>();
        foreach (var client in clients)
        {
            clientDictionary[client.PhoneNumber] = client;
        }
        return clientDictionary;
    }

    public List<Employee> GenerateEmployees(int count)
    {
       var employeeFaker = new Faker<Employee>("ru")
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.PhoneNumber, f => $"+373 777 66 {f.Random.Number(100, 999)}")
            .RuleFor(e => e.Position, f => "Сотрудник")
            .RuleFor(e => e.BirthDay, f => f.Date.Past(18))
            .RuleFor(e => e.Contract, f => "Контракт для сотрудника")
            .RuleFor(e => e.Salary, f => f.Random.Number(1000, 50000));
        return employeeFaker.Generate(count);
    }
}