using System.Linq.Expressions;
using BankSystem.Domain.Models;
using BankSystem.App.Interfaces;
using BankSystem.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private readonly BankSystemDbContext _context;

        public EmployeeStorage(BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<ICollection<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task AddAsync(Employee item)
        {
            if (await _context.Employees.AnyAsync(e => e.Equals(item)))
            {
                throw new InvalidOperationException("Сотрудник с таким номером телефона уже существует.");
            }

            await _context.Employees.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, Employee item)
        {
            var existingEmployee = await GetAsync(id);
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

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingEmployee = await GetAsync(id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Сотрудник не найден.");
            }

            _context.Employees.Remove(existingEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Employee>> GetByFilterAsync(Expression<Func<Employee, bool>> filter)
        {
            return await _context.Employees
                .Include(e => e.Position) 
                .Where(filter) 
                .ToListAsync();
        }
    }
}