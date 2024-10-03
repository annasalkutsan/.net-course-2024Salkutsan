using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage
{
    private Dictionary<Client, List<Account>> _clientAccounts;

    public ClientStorage()
    {
        _clientAccounts = new Dictionary<Client, List<Account>>();
    }
    
    public void AddClient(Client client, List<Account> accounts)
    {
        _clientAccounts[client] = accounts;
    }

    public void AddClients(Dictionary<Client, List<Account>> clients)
    {
        foreach (var client in clients)
        {
            _clientAccounts[client.Key] = client.Value;
        }
    }

    public Client GetYoungestClient()
    {
        return _clientAccounts.Keys.OrderBy(c => c.BirthDay).FirstOrDefault();
    }

    public Client GetOldestClient()
    {
        return _clientAccounts.Keys.OrderByDescending(c => c.BirthDay).FirstOrDefault();
    }

    public double GetAverageAgeClient()
    {
        return _clientAccounts.Any()
            ? _clientAccounts.Keys
                .Select(c => DateTime.Now.Year - c.BirthDay.Year - (DateTime.Now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0))
                .Average()
            : 0;
    }
    
    public List<Client> GetAllClients()
    {
        return _clientAccounts.Keys.ToList();
    }

    public List<Account> GetClientAccounts(Client client)
    {
        return _clientAccounts.TryGetValue(client, out var accounts) ? accounts : new List<Account>();
    }
}