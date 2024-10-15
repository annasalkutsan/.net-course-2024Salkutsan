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

        public Employee GetEmployee(Guid id)
        {
            var employee = _employeeStorage.Get(id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Сотрудник не найден.");
            }
            return employee;
        }

        public ICollection<Employee> GetAllEmployees()
        {
            return _employeeStorage.GetAll();
        }

        public void AddEmployee(Employee employee)
        {
            ValidateEmployee(employee);
            _employeeStorage.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            ValidateEmployee(employee);
            _employeeStorage.Update(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            _employeeStorage.Delete(employee);
        }

        public ICollection<Employee> GetEmployeesByFilter(
            string lastName = null, 
            string phoneNumber = null, 
            string positionName = null,
            int pageNumber = 1, // номер страницы
            int pageSize = 10)  // количество записей на странице
        {
            // все сотрудники по фильтру
            var query = _employeeStorage.GetByFilter(e =>
                (string.IsNullOrWhiteSpace(lastName) || e.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || e.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrWhiteSpace(positionName) || (e.Position != null && e.Position.Title.Contains(positionName, StringComparison.OrdinalIgnoreCase))));

            // пагинация
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        
        private void ValidateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть нулевым.");
            }

            if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            {
                throw new PhoneNumberException();
            }

            if (_employeeStorage.GetAll().Any(e => e.Equals(employee) && e.Id != employee.Id))
            {
                throw new InvalidOperationException("Сотрудник с таким номером телефона уже существует.");
            }
        }
    }
}
