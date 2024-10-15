using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class PositionStorage: IStorage<Domain.Models.PositionStorage>
{
    private readonly BankSystemDbContext _context;

    public PositionStorage(BankSystemDbContext context)
    {
        _context = context;
    }

    public Domain.Models.PositionStorage Get(Guid id)
    {
        return _context.Positions.Find(id);
    }

    public ICollection<Domain.Models.PositionStorage> GetAll()
    {
        return _context.Positions.ToList();
    }

    public void Add(Domain.Models.PositionStorage item)
    {
        _context.Positions.Add(item);
        _context.SaveChanges();
    }

    public void Update(Domain.Models.PositionStorage item)
    {
        var existingPosition = Get(item.Id);
        if (existingPosition == null)
        {
            throw new KeyNotFoundException("Должность не найдена.");
        }

        existingPosition.Title = item.Title;

        _context.SaveChanges();
    }

    public void Delete(Domain.Models.PositionStorage item)
    {
        var existingPosition = Get(item.Id);
        if (existingPosition == null)
        {
            throw new KeyNotFoundException("Должность не найдена.");
        }

        _context.Positions.Remove(existingPosition);
        _context.SaveChanges();
    }
}