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

    public void Add(Position item)
    {
        _context.Positions.Add(item);
        _context.SaveChanges();
    }

    public void Update(Position item)
    {
        var existingPosition = Get(item.Id);
        if (existingPosition == null)
        {
            throw new KeyNotFoundException("Должность не найдена.");
        }

        existingPosition.Title = item.Title;

        _context.SaveChanges();
    }

    public void Delete(Position item)
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