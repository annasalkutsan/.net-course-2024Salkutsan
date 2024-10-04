using BankSystem.App.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class ClientService
{
    private readonly ClientStorage _clientStorage; 

    public ClientService(ClientStorage clientStorage)
    {
        _clientStorage = clientStorage; 
    }
    
    public void AddClient(Client client)
    {
        if (client.BirthDay > DateTime.Now.AddYears(-18))
        {
            throw new AgeException(); 
        }

        if (string.IsNullOrWhiteSpace(client.Passport))
        {
            throw new PassportException(); 
        }

        _clientStorage.AddClient(client);
    }

    public void AddClient(Client client, List<Account> accounts)
    {
        if (client.BirthDay > DateTime.Now.AddYears(-18))
        {
            throw new AgeException();
        }

        if (string.IsNullOrWhiteSpace(client.Passport))
        {
            throw new PassportException(); 
        }

        _clientStorage.AddClient(client, accounts);
    }

    public void AddClients(Dictionary<Client, List<Account>> clients)
    {
        foreach (var client in clients)
        {
            if (client.Key.BirthDay > DateTime.Now.AddYears(-18))
            {
                throw new AgeException(); 
            }

            if (string.IsNullOrWhiteSpace(client.Key.Passport))
            {
                throw new PassportException(); 
            }
        }

        _clientStorage.AddClients(clients);
    }

    public void AddAccountToClient(Client client, Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
        }

        _clientStorage.AddAccountToClient(client, account);
    }

    public void EditAccount(Client client, Account oldAccount, Account newAccount)
    {
        if (oldAccount == null || newAccount == null)
        {
            throw new ArgumentNullException("Старый или новый лицевой счет не может быть нулевым.");
        }

        if (oldAccount.Currency != newAccount.Currency) 
        {
            throw new InvalidOperationException("Изменение валюты счета невозможно.");
        }

        oldAccount.Amount = newAccount.Amount;
        _clientStorage.EditAccount(client, oldAccount, newAccount); 
    }

    public void EditClient(Client oldClient, Client newClient)
    {
        if (newClient.BirthDay > DateTime.Now.AddYears(-18))
        {
            throw new AgeException();
        }

        if (string.IsNullOrWhiteSpace(newClient.Passport))
        {
            throw new PassportException();
        }

        _clientStorage.EditClient(oldClient, newClient); 
    }
    
    public void RemoveClient(Client client)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client), "Клиент не может быть нулевым."); 
        }

        _clientStorage.RemoveClient(client); 
    }

    public void RemoveAccountFromClient(Client client, Account account)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client), "Клиент не может быть нулевым."); 
        }

        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
        }

        _clientStorage.RemoveAccountFromClient(client, account); 
    }

    public List<Client> GetClientsByFilter(string fullName = null, string phoneNumber = null, string passport = null, DateTime? birthStart = null, DateTime? birthEnd = null)
    {
        return _clientStorage.GetClientsByFilter(fullName, phoneNumber, passport, birthStart, birthEnd);
    }

    public List<Client> GetAllClients()
    {
        return _clientStorage.GetAllClients();
    }
}