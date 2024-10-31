using System.Linq.Expressions;

namespace BankSystem.App.Interfaces;

public interface IStorage<T>
{
    Task<T> GetAsync(Guid id);
    Task<ICollection<T>> GetAllAsync();
    Task<ICollection<T>> GetByFilterAsync(Expression<Func<T, bool>> filter); // изменено
    Task AddAsync(T item);
    Task UpdateAsync(Guid id, T item);
    Task DeleteAsync(Guid id);
}