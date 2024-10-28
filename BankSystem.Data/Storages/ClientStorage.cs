using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private readonly BankSystemDbContext _context;

        public ClientStorage(BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetAsync(Guid id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<ICollection<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task AddAsync(Client item)
        {
            if (await _context.Clients.AnyAsync(c => c.Equals(item)))
            {
                throw new InvalidOperationException("Клиент с таким паспортом уже существует.");
            }

            var defaultAccount = new Account(new Currency("USD", "Доллар США"), 0)
            {
                ClientId = item.Id
            };

            item.Accounts.Add(defaultAccount);
            
            await _context.Clients.AddAsync(item);
            await _context.Accounts.AddAsync(defaultAccount);
            
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, Client item)
        {
            var existingClient = await GetAsync(id);
            if (existingClient == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }

            existingClient.FirstName = item.FirstName;
            existingClient.LastName = item.LastName;
            existingClient.PhoneNumber = item.PhoneNumber;
            existingClient.BirthDay = item.BirthDay;
            existingClient.Passport = item.Passport;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingClient = await GetAsync(id);
    
            if (existingClient == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }
            
            _context.Clients.Remove(existingClient);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Client>> GetByFilterAsync(Func<Client, bool> filter)
        {
            return await Task.Run(() => _context.Clients.AsQueryable()
                .Where(filter)
                .ToList());
        }

        public async Task<ICollection<Account>> GetAccountsByClientIdAsync(Guid clientId)
        {
            return await _context.Accounts
                .Where(a => a.ClientId == clientId)
                .ToListAsync();
        }

        public async Task AddAccountAsync(Guid clientId, Account account)
        {
            var client = await GetAsync(clientId);
            if (client == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }

            account.ClientId = clientId;
            client.Accounts.Add(account);

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
    
            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Аккаунт не найден.");
            }

            existingAccount.Amount = account.Amount;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Аккаунт не найден.");
            }

            _context.Accounts.Remove(existingAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageAgeClientAsync()
        {
            var now = DateTime.Now;

            return await _context.Clients.AnyAsync()
                ? await _context.Clients
                    .Select(c => now.Year - c.BirthDay.Year - (now.DayOfYear < c.BirthDay.DayOfYear ? 1 : 0))
                    .AverageAsync()
                : 0;
        }
    }
}