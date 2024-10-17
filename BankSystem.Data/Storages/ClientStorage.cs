using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private readonly BankSystemDbContext _context;

        public ClientStorage(BankSystemDbContext context)
        {
            _context = context;
        }
        public Client Get(Guid id)
        {
            return _context.Clients.Find(id);
        }

        public ICollection<Client> GetAll()
        {
            return _context.Clients.ToList();
        }

        public void Add(Client item)
        {
            if (_context.Clients.Any(c => c.Equals(item)))
            {
                throw new InvalidOperationException("Клиент с таким паспортом уже существует.");
            }

            var defaultAccount = new Account(new Currency("USD", "Доллар США"), 0)
            {
                ClientId = item.Id 
            };

            item.Accounts.Add(defaultAccount);
            
            _context.Clients.Add(item);
            _context.Accounts.Add(defaultAccount);
            
            _context.SaveChanges();
        }

        public void Update(Guid id, Client item)
        {
            var existingClient = Get(id);
            if (existingClient == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }

            existingClient.FirstName = item.FirstName;
            existingClient.LastName = item.LastName;
            existingClient.PhoneNumber = item.PhoneNumber;
            existingClient.BirthDay = item.BirthDay;
            existingClient.Passport = item.Passport; 

            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existingClient = Get(id);
    
            if (existingClient == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }
            
            _context.Clients.Remove(existingClient);

            _context.SaveChanges();
        }
        
        public ICollection<Client> GetByFilter(Func<Client, bool> filter)
        {
            return _context.Clients.AsQueryable()
                .Where(filter)
                .ToList();
        }

        public ICollection<Account> GetAccountsByClientId(Guid clientId)
        {
            return _context.Accounts
                .Where(a => a.ClientId == clientId)
                .ToList();
        }

        public void AddAccount(Guid clientId, Account account)
        {
            var client = Get(clientId);
            if (client == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }

            account.ClientId = clientId;

            client.Accounts.Add(account);
            
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }


        public void UpdateAccount(Account account)
        {
            var existingAccount = _context.Accounts.FirstOrDefault(a => a.Id == account.Id);
    
            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Аккаунт не найден.");
            }

            existingAccount.Amount = account.Amount;

            _context.SaveChanges();
        }
        
        public void DeleteAccount(Guid accountId)
        {
            var existingAccount = _context.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Аккаунт не найден.");
            }

            _context.Accounts.Remove(existingAccount);

            _context.SaveChanges();
        }
        
        public double GetAverageAgeClient()
        {
            var now = DateTime.Now;
    
            return _context.Clients.Any()
                ? _context.Clients
                    .Select(c => now.Year - c.BirthDay.Year - (now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0))
                    .Average()
                : 0;
        }

    }
}