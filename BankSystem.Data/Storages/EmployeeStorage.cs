﻿using BankSystem.Domain.Models;
using BankSystem.App.Interfaces;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private List<Employee> _employees;

        public EmployeeStorage()
        {
            _employees = new List<Employee>();
        }

        public List<Employee> Get(Func<Employee, bool> filter)
        {
            return _employees.Where(filter).ToList();
        }

        public void Add(Employee item)
        {
            _employees.Add(item);
        }
        
        public void AddCollection(List<Employee> items)
        {
            _employees.AddRange(items);
        }

        public void Update(Employee item)
        {
            var existingEmployee = _employees.FirstOrDefault(e=>e.Equals(item));
            if (existingEmployee == null)
            {
                throw new InvalidOperationException("Сотрудник не найден в списке.");
            }
            typeof(Employee).GetProperties()
                .ToList()
                .ForEach(p => p.SetValue(existingEmployee, p.GetValue(item)));
        }

        public void Delete(Employee item)
        {
            if (!_employees.Remove(item))
            {
                throw new InvalidOperationException("Сотрудник не найден в списке.");
            }
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
                .Select(e => DateTime.Now.Year - e.BirthDay.Year - (DateTime.Now.DayOfYear < e.BirthDay.DayOfYear ? 1 : 0))
                .Average();
        }
    }
}