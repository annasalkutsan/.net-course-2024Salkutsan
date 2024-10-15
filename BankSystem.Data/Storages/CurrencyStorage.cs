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

    public void Add(Currency item)
    {
        _context.Currencies.Add(item);
        _context.SaveChanges();
    }

    public void Update(Currency item)
    {
        var existingCurrency = Get(item.Id);
        if (existingCurrency == null)
        {
            throw new KeyNotFoundException("Валюта не найдена.");
        }

        existingCurrency.Code = item.Code;
        existingCurrency.Name = item.Name;

        _context.SaveChanges();
    }

    public void Delete(Currency item)
    {
        var existingCurrency = Get(item.Id);
        if (existingCurrency == null)
        {
            throw new KeyNotFoundException("Валюта не найдена.");
        }

        _context.Currencies.Remove(existingCurrency);
        _context.SaveChanges();
    }
}