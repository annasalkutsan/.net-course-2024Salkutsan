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

        public async Task<Employee> GetEmployeeAsync(Guid id)
        {
            var employee = await _employeeStorage.GetAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Сотрудник не найден.");
            }
            return employee;
        }

        public async Task<ICollection<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeStorage.GetAllAsync();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await ValidateEmployeeAsync(employee);
            await _employeeStorage.AddAsync(employee);
        }

        public async Task UpdateEmployeeAsync(Guid idEmployee, Employee employee)
        {
            await ValidateEmployeeAsync(employee);
            await _employeeStorage.UpdateAsync(idEmployee, employee);
        }

        public async Task DeleteEmployeeAsync(Guid idEmployee)
        {
            await _employeeStorage.DeleteAsync(idEmployee);
        }

        public async Task<ICollection<Employee>> GetEmployeesByFilterAsync(
            string lastName = null, 
            string phoneNumber = null, 
            Guid? positionId = null,
            int pageNumber = 1, 
            int pageSize = 10)
        {
            var employees = await _employeeStorage.GetByFilterAsync(e =>
                (string.IsNullOrWhiteSpace(lastName) || e.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || e.PhoneNumber.Contains(phoneNumber)) &&
                (!positionId.HasValue || e.PositionId == positionId));

            return employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        
        private async Task ValidateEmployeeAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть нулевым.");
            }

            if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            {
                throw new PhoneNumberException();
            }

            var employees = await _employeeStorage.GetAllAsync();
            if (employees.Any(e => e.Equals(employee) && e.Id != employee.Id))
            {
                throw new InvalidOperationException("Сотрудник с таким номером телефона уже существует.");
            }
        }
    }
}