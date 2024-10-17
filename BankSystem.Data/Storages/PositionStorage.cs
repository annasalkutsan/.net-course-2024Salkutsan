using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class PositionStorage: IStorage<Position>
{
    private readonly BankSystemDbContext _context;

    public PositionStorage(BankSystemDbContext context)
    {
        _context = context;
    }

    public Position Get(Guid id)
    {
        return _context.Positions.Find(id);
    }

    public ICollection<Position> GetAll()
    {
        return _context.Positions.ToList();
    }

    public ICollection<Position> GetByFilter(Func<Position, bool> filter)
    {
        return _context.Positions.AsQueryable()
            .Where(filter)
            .ToList();
    }

    public void Add(Position item)
    {
        _context.Positions.Add(item);
        _context.SaveChanges();
    }

    public void Update(Guid id, Position item)
    {
        var existingPosition = Get(id);
        if (existingPosition == null)
        {
            throw new KeyNotFoundException("Должность не найдена.");
        }

        existingPosition.Title = item.Title;

        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingPosition = Get(id);
        if (existingPosition == null)
        {
            throw new KeyNotFoundException("Должность не найдена.");
        }

        _context.Positions.Remove(existingPosition);
        _context.SaveChanges();
    }
}