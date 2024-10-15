namespace BankSystem.App.Interfaces;

public interface IStorage<T>
{
    T Get(Guid id);
    ICollection<T> GetAll(); 
    void Add(T item);
    void Update(T item); 
    void Delete(T item); 
}