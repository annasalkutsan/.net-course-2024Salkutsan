using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IEmployeeStorage : IStorage<Employee, List<Employee>>
{
    void AddCollection(List<Employee> employees);
}
