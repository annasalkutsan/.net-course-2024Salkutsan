using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;

namespace BankSystem.Data.Storages;

public class CurrencyStorage: IStorage<Currency>
{
    private readonly BankSystemDbContext _context;

    public CurrencyStorage(BankSystemDbContext context)
    {
        _context = context;
    }

    public Currency Get(Guid id)
    {
        return _context.Currencies.Find(id);
    }

    public ICollection<Currency> GetAll()
    {
        return _context.Currencies.ToList();
    }

    public ICollection<Currency> GetByFilter(Func<Currency, bool> filter)
    {
        return _context.Currencies.AsQueryable()
            .Where(filter)
            .ToList();
    }
    
    public void Add(Currency item)
    {
        _context.Currencies.Add(item);
        _context.SaveChanges();
    }

    public void Update(Guid id, Currency item)
    {
        var existingCurrency = Get(id);
        if (existingCurrency == null)
        {
            throw new KeyNotFoundException("Валюта не найдена.");
        }

        existingCurrency.Code = item.Code;
        existingCurrency.Name = item.Name;

        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingCurrency = Get(id);
        if (existingCurrency == null)
        {
            throw new KeyNotFoundException("Валюта не найдена.");
        }

        _context.Currencies.Remove(existingCurrency);
        _context.SaveChanges();
    }
}