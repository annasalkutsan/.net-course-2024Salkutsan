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

    public void AddEmployee(List<Employee> employees)
    {
        _employees.AddRange(employees);
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
        return new List<Employee>(_employees);
    }
    
    public List<Employee> GetEmployeesByFilter(string lastName = null, string phoneNumber = null, string position = null)
    {
        return _employees.Where(e =>
                (string.IsNullOrWhiteSpace(lastName) || e.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || e.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrWhiteSpace(position) || e.Position.Contains(position, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }
    
    public void EditEmployee(Employee oldEmployee, Employee newEmployee)
    {
        if (oldEmployee == null || newEmployee == null)
        {
            throw new ArgumentNullException("Старый или новый сотрудник не может быть нулевым.");
        }

        var index = _employees.IndexOf(oldEmployee);
        if (index < 0)
        {
            throw new InvalidOperationException("Сотрудник не найден в списке.");
        }

        _employees[index] = newEmployee;
    }
    
    public void RemoveEmployee(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть нулевым.");
        }

        if (!_employees.Remove(employee))
        {
            throw new InvalidOperationException("Сотрудник не найден в списке.");
        }
    }
    
    public void RemoveEmployees(List<Employee> employees)
    {
        if (employees == null || employees.Count == 0)
        {
            throw new ArgumentNullException(nameof(employees), "Список сотрудников не может быть нулевым или пустым.");
        }

        foreach (var employee in employees)
        {
            RemoveEmployee(employee); 
        }
    }

}