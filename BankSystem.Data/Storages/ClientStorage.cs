using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage
{
    private List<Client> _clients;

    public ClientStorage()
    {
        _clients = new List<Client>();
    }

    public void AddClient(Client client)
    {
        _clients.Add(client);
    }

    public void AddClient(List<Client> clients)
    {
        _clients.AddRange(clients);
    }
    
    public Client GetYoungestClient()
    {
        return _clients.OrderBy(c => c.BirthDay).FirstOrDefault();
    }

    public Client GetOldestClient()
    {
        return _clients.OrderByDescending(c => c.BirthDay).FirstOrDefault();
    }
    
    public double GetAverageAgeClient()
    {
        if (_clients.Count == 0) return 0;
        
        return _clients
            .Select(c => 
                DateTime.Now.Year - c.BirthDay.Year - (DateTime.Now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0))
            .Average();
    }

    public List<Client> GetAllClients()
    {
        return new List<Client>(_clients);
    }
}