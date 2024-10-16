namespace BankSystem.App.Interfaces;

public interface IStorage<T>
{
    T Get(Guid id);
    ICollection<T> GetAll(); 
    ICollection<T> GetByFilter(Func<T, bool> filter);
    void Add(T item);
    void Update(Guid id, T item); 
    void Delete(Guid id); 
}