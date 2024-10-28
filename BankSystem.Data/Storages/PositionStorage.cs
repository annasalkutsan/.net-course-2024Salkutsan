using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storages
{
    public class PositionStorage : IStorage<Position>
    {
        private readonly BankSystemDbContext _context;

        public PositionStorage(BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Position> GetAsync(Guid id)
        {
            return await _context.Positions.FindAsync(id);
        }

        public async Task<ICollection<Position>> GetAllAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<ICollection<Position>> GetByFilterAsync(Func<Position, bool> filter)
        {
            return await Task.Run(() => _context.Positions.AsQueryable().Where(filter).ToList());
        }

        public async Task AddAsync(Position item)
        {
            await _context.Positions.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, Position item)
        {
            var existingPosition = await GetAsync(id);
            if (existingPosition == null)
            {
                throw new KeyNotFoundException("Должность не найдена.");
            }

            existingPosition.Title = item.Title;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingPosition = await GetAsync(id);
            if (existingPosition == null)
            {
                throw new KeyNotFoundException("Должность не найдена.");
            }

            _context.Positions.Remove(existingPosition);
            await _context.SaveChangesAsync();
        }
    }
}