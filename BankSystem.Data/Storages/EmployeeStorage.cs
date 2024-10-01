using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class EmployeeStorage
{
    private List<Employee> _employees;

    public EmployeeStorage()
    {
        _employees = new List<Employee>();
    }

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }

    public Employee GetYoungestEmployee()
    {
        return _employees.OrderBy(e => e.BirthDay).FirstOrDefault();
    }

    public Employee GetOldestEmployee()
    {
        return _employees.OrderByDescending(e => e.BirthDay).FirstOrDefault();
    }

    public double GetAverageAgeEmployee()
    {
        if (_employees.Count == 0) return 0;

        return _employees
            .Select((e =>
                DateTime.Now.Year - e.BirthDay.Year - (DateTime.Now.DayOfYear < e.BirthDay.DayOfYear ? 1 : 0)))
            .Average();
    }

    public List<Employee> GetAllEmployees()
    {
        return _employees;
    }
}