using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storages
{
    public class CurrencyStorage : IStorage<Currency>
    {
        private readonly BankSystemDbContext _context;

        public CurrencyStorage(BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Currency> GetAsync(Guid id)
        {
            return await _context.Currencies.FindAsync(id);
        }

        public async Task<ICollection<Currency>> GetAllAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task<ICollection<Currency>> GetByFilterAsync(Func<Currency, bool> filter)
        {
            return await Task.Run(() => _context.Currencies.AsQueryable().Where(filter).ToList());
        }

        public async Task AddAsync(Currency item)
        {
            await _context.Currencies.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, Currency item)
        {
            var existingCurrency = await GetAsync(id);
            if (existingCurrency == null)
            {
                throw new KeyNotFoundException("Валюта не найдена.");
            }

            existingCurrency.Code = item.Code;
            existingCurrency.Name = item.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingCurrency = await GetAsync(id);
            if (existingCurrency == null)
            {
                throw new KeyNotFoundException("Валюта не найдена.");
            }

            _context.Currencies.Remove(existingCurrency);
            await _context.SaveChangesAsync();
        }
    }
}