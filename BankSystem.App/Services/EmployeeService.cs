using BankSystem.App.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class EmployeeService
{
    private readonly EmployeeStorage _employeeStorage;

    public EmployeeService()
    {
        _employeeStorage = new EmployeeStorage();
    }

    public void AddEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Position))
            throw new PositionException();

        if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            throw new PhoneNumberException();

        _employeeStorage.AddEmployee(employee);
    }

    public void AddEmployees(List<Employee> employees)
    {
        if (employees == null || employees.Count == 0)
            throw new ArgumentException("Список сотрудников не может быть пустым.", nameof(employees));

        foreach (var employee in employees)
        {
            if (string.IsNullOrWhiteSpace(employee.Position))
                throw new PositionException();

            if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
                throw new PhoneNumberException();
        }

        _employeeStorage.AddEmployee(employees);
    }

    public List<Employee> GetAllEmployees()
    {
        return _employeeStorage.GetAllEmployees();
    }

    public List<Employee> GetEmployeesByFilter(string lastName = null, string phoneNumber = null, string position = null)
    {
        if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Хотя бы один фильтр должен быть указан.");

        return _employeeStorage.GetEmployeesByFilter(lastName, phoneNumber, position);
    }

    public void EditEmployee(Employee oldEmployee, Employee newEmployee)
    {
        if (oldEmployee == null || newEmployee == null)
            throw new ArgumentNullException("Старый и новый сотрудник не могут быть нулевыми.");

        if (string.IsNullOrWhiteSpace(newEmployee.Position))
            throw new PositionException();

        if (string.IsNullOrWhiteSpace(newEmployee.PhoneNumber))
            throw new PhoneNumberException();

        _employeeStorage.EditEmployee(oldEmployee, newEmployee);
    }

    public void RemoveEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть нулевым.");

        _employeeStorage.RemoveEmployee(employee);
    }

    public void RemoveEmployees(List<Employee> employees)
    {
        if (employees == null || employees.Count == 0)
            throw new ArgumentException("Список сотрудников не может быть пустым.", nameof(employees));

        foreach (var employee in employees)
        {
            if (employee == null)
                throw new ArgumentException("Каждый сотрудник в списке должен быть не нулевым.", nameof(employees));
        }

        _employeeStorage.RemoveEmployees(employees);
    }
}