using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IEmployeeStorage : IStorage<Employee>
{
    ICollection<Employee> GetByFilter(Func<Employee, bool> filter);
}
