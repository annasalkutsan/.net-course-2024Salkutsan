using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private Dictionary<Client, List<Account>> _clientAccounts;

        public ClientStorage()
        {
            _clientAccounts = new Dictionary<Client, List<Account>>();
        }

        public Dictionary<Client, List<Account>> Get(Func<Client, bool> filter)
        {
            return _clientAccounts
                .Where(kvp => filter(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public void Add(Client client)
        {
            if (_clientAccounts.ContainsKey(client))
            {
                throw new InvalidOperationException("Клиент уже существует в хранилище.");
            }
            var defaultAccount = new Account(new Currency("USD", "Доллар США"), 0);
            _clientAccounts[client] = new List<Account> { defaultAccount };
        }

        public void Add<TKey, TValue>(Dictionary<TKey, TValue> clients)
        {
            if (clients is Dictionary<Client, List<Account>> clientAccounts)
            {
                foreach (var client in clientAccounts)
                {
                    if (client.Value == null || !client.Value.Any())
                    {
                        var defaultAccount = new Account(new Currency("USD", "Доллар США"), 0);
                        _clientAccounts[client.Key] = new List<Account> { defaultAccount };
                    }
                    else
                    {
                        _clientAccounts[client.Key] = client.Value;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Неверный тип словаря. Ожидался Dictionary<Client, List<Account>>.");
            }
        }

        public void Update(Client updatedClient)
        {
            var existingClient = _clientAccounts.Keys.FirstOrDefault(c => c.Passport == updatedClient.Passport);
            if (existingClient != null)
            {
                var accounts = _clientAccounts[existingClient]
                    .Select(account => new Account(account.Currency, account.Amount))
                    .ToList();
                _clientAccounts.Remove(existingClient);
                _clientAccounts[updatedClient] = accounts;
            }
            else
            {
                throw new KeyNotFoundException("Клиент с указанным паспортом не найден.");
            }
        }

        public void Delete(Client client)
        {
            _clientAccounts.Remove(client);
        }

        public void AddAccount(Client client, Account account)
        {
            if (_clientAccounts.ContainsKey(client))
            {
                _clientAccounts[client].Add(account);
            }
        }

        public void UpdateAccount(Client client, Account account)
        {
            if (_clientAccounts.TryGetValue(client, out var accounts))
            {
                var index = accounts.IndexOf(account);
                if (index != -1)
                {
                    accounts[index] = account;
                }
            }
        }

        public void DeleteAccount(Client client, Account account)
        {
            if (_clientAccounts.ContainsKey(client))
            {
                var accounts = _clientAccounts[client];
                accounts.Remove(account); 
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
}