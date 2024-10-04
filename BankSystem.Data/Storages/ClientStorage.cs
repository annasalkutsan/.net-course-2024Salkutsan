using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage
{
    private Dictionary<Client, List<Account>> _clientAccounts;

    public ClientStorage()
    {
        _clientAccounts = new Dictionary<Client, List<Account>>();
    }
    
    public virtual void AddClient(Client client)
    {
        // дефолтный долларовый лицевой счет
        var defaultAccount = new Account(new Currency("USD", "Доллар США"), 0);
        _clientAccounts[client] = new List<Account> { defaultAccount };
    }

    public virtual void AddClient(Client client, List<Account> accounts)
    {
        _clientAccounts[client] = accounts;
    }
    
    public virtual void AddClients(Dictionary<Client, List<Account>> clients)
    {
        foreach (var client in clients)
        {
            _clientAccounts[client.Key] = client.Value;
        }
    }

    public virtual void AddAccountToClient(Client client, Account account)
    {
        if (_clientAccounts.ContainsKey(client))
        {
            _clientAccounts[client].Add(account);
        }
    }

    public virtual void EditAccount(Client client, Account oldAccount, Account newAccount)
    {
        if (_clientAccounts.TryGetValue(client, out var accounts))
        {
            var index = accounts.IndexOf(oldAccount);
            if (index != -1)
            {
                accounts[index] = newAccount;
            }
        }
    }

    public virtual void EditClient(Client oldClient, Client newClient)
    {
        if (_clientAccounts.ContainsKey(oldClient))
        {
            var accounts = _clientAccounts[oldClient];
            _clientAccounts.Remove(oldClient);
            _clientAccounts[newClient] = accounts;
        }
    }

    public virtual void RemoveClient(Client client)
    {
        _clientAccounts.Remove(client);
    }
    
    public virtual void RemoveAccountFromClient(Client client, Account account)
    {
        if (_clientAccounts.TryGetValue(client, out var accounts))
        {
            accounts.Remove(account);
        }
    }

    public virtual List<Client> GetClientsByFilter(string fullName = null, string phoneNumber = null, string passport = null, DateTime? birthStart = null, DateTime? birthEnd = null)
    {
        return _clientAccounts.Keys.Where(client =>
            (string.IsNullOrEmpty(fullName) || $"{client.FirstName} {client.LastName}".Contains(fullName)) &&
            (string.IsNullOrEmpty(phoneNumber) || client.PhoneNumber.Contains(phoneNumber)) &&
            (string.IsNullOrEmpty(passport) || client.Passport.Contains(passport)) &&
            (!birthStart.HasValue || client.BirthDay >= birthStart.Value) &&
            (!birthEnd.HasValue || client.BirthDay <= birthEnd.Value)).ToList();
    }

    public virtual Client GetYoungestClient()
    {
        return _clientAccounts.Keys.OrderBy(c => c.BirthDay).FirstOrDefault();
    }

    public virtual Client GetOldestClient()
    {
        return _clientAccounts.Keys.OrderByDescending(c => c.BirthDay).FirstOrDefault();
    }

    public virtual double GetAverageAgeClient()
    {
        return _clientAccounts.Any()
            ? _clientAccounts.Keys
                .Select(c => DateTime.Now.Year - c.BirthDay.Year - (DateTime.Now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0))
                .Average()
            : 0;
    }
    
    public virtual List<Client> GetAllClients()
    {
        return _clientAccounts.Keys.ToList();
    }

    public virtual List<Account> GetClientAccounts(Client client)
    {
        return _clientAccounts.TryGetValue(client, out var accounts) ? accounts : new List<Account>();
    }
}