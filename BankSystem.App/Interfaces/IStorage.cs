namespace BankSystem.App.Interfaces;

public interface IStorage<T, TResult>
{
    TResult Get(Func<T, bool> filter);
    void Add(T item);
    void Add(List<T> items)
    {
        throw new NotImplementedException("Это хранилище не поддерживает добавление коллекции.");
    }
    void Add<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
    {
        throw new NotImplementedException("Это хранилище не поддерживает добавление словаря.");
    }
    void Update(T item);
    void Delete(T item);
}