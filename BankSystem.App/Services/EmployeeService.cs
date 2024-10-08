using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeStorage _employeeStorage;

        public EmployeeService(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }
        
        public List<Employee> GetEmployeesByFilter(string lastName = null, string phoneNumber = null, string position = null)
        {
            return _employeeStorage.Get(e =>
                (string.IsNullOrWhiteSpace(lastName) || e.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || e.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrWhiteSpace(position) || e.Position.Contains(position, StringComparison.OrdinalIgnoreCase)));
        }
        
        public void Add(Employee employee)
        {
            ValidateEmployee(employee);
            _employeeStorage.Add(employee);
        }

        public void Add(List<Employee> employees)
        {
            if (employees == null || employees.Count == 0)
                throw new ArgumentException("Список сотрудников не может быть пустым.", nameof(employees));

            foreach (var employee in employees)
            {
                ValidateEmployee(employee);
            }

            _employeeStorage.Add(employees);
        }

        public void Update(Employee employee)
        {
            ValidateEmployee(employee);
            _employeeStorage.Update(employee);
        }

        public void Delete(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть нулевым.");

            _employeeStorage.Delete(employee);
        }

        private void ValidateEmployee(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Position))
                throw new PositionException();

            if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
                throw new PhoneNumberException();
        }
    }
}
