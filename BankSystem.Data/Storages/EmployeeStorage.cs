using BankSystem.Domain.Models;
using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private readonly BankSystemDbContext _context;

        public EmployeeStorage(BankSystemDbContext context)
        {
            _context = context;
        }

        public Employee Get(Guid id)
        {
            return _context.Employees.Find(id);
        }

        public ICollection<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public void Add(Employee item)
        {
            if (_context.Employees.Any(e => e.Equals(item)))
            {
                throw new InvalidOperationException("Сотрудник с таким номером телефона уже существует.");
            }

            _context.Employees.Add(item);
            _context.SaveChanges();
        }

        public void Update(Employee item)
        {
            var existingEmployee = Get(item.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Сотрудник не найден.");
            }

            existingEmployee.Contract = item.Contract;
            existingEmployee.Salary = item.Salary;
            existingEmployee.FirstName = item.FirstName;
            existingEmployee.LastName = item.LastName;
            existingEmployee.PhoneNumber = item.PhoneNumber;
            existingEmployee.BirthDay = item.BirthDay;
           
            if (item.PositionId.HasValue)
            {
                existingEmployee.PositionId = item.PositionId.Value;
            }
            
            _context.SaveChanges();
        }

        public void Delete(Employee item)
        {
            var existingEmployee = Get(item.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Сотрудник не найден.");
            }

            _context.Employees.Remove(existingEmployee);
            _context.SaveChanges();
        }

        public ICollection<Employee> GetByFilter(Func<Employee, bool> filter)
        {
            return _context.Employees.AsQueryable()
                .Where(filter)
                .ToList();
        }
    }
}