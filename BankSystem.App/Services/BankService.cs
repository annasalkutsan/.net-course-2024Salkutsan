using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class BankService
{
    /// <summary>
    /// Расчет зарплаты владельцев банка
    /// </summary>
    /// <param name="bankProfit"></param>
    /// <param name="bankExpenses"></param>
    /// <param name="owners"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public int CalculateOwnerSalary(int bankProfit, int bankExpenses, List<Person> owners)
    {
        foreach (var owner in owners)
        {
            if (owner is not Employee)
            {
                throw new ArgumentException("Владельцы должны быть сотрудниками банка.");
            }
        }
        
        int totalProfit = bankProfit - bankExpenses;
        if (owners.Count == 0)
        {
            throw new ArgumentException("Количество владельцев не может быть равно нулю.");
        }
        
        int ownerSalary = totalProfit / owners.Count;
        return ownerSalary;
    }
    
    /// <summary>
    /// Преобразования клиента банка в сотрудника
    /// </summary>
    /// <param name="client"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Employee ConvertClientToEmployee(Client client, string position)
    {
        return new Employee
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            PhoneNumber = client.PhoneNumber,
            BirthDay = client.BirthDay,
            Position = position,
            Contract = "Новый контракт для сотрудника"
        };
    }
}