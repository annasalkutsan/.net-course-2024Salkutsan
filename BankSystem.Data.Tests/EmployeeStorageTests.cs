using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankSystem.Data.Tests
{
    public class EmployeeStorageTests
    {
        private readonly EmployeeStorage _employeeStorage;
        private readonly TestDataGenerator _dataGenerator;

        public EmployeeStorageTests()
        {
            _employeeStorage = new EmployeeStorage();
            _dataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void AddEmployee_PositiveTest()
        {
            var employee = _dataGenerator.GenerateEmployees(1).First();

            _employeeStorage.Add(employee);

            Assert.Single(_employeeStorage.Get(e => true));
        }

        [Fact]
        public void AddEmployeeCollection_PositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(5);

            _employeeStorage.Add(employees);

            Assert.Equal(5, _employeeStorage.Get(e => true).Count);
        }

        [Fact]
        public void GetYoungestEmployee_PositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(10);
            _employeeStorage.Add(employees);

            var youngestEmployee = _employeeStorage.GetYoungestEmployee();

            Assert.NotNull(youngestEmployee);
            Assert.Equal(employees.OrderBy(c => c.BirthDay).First(), youngestEmployee);
        }

        [Fact]
        public void GetOldestEmployee_PositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(10);
            _employeeStorage.Add(employees);

            var oldestEmployee = _employeeStorage.GetOldestEmployee();

            Assert.NotNull(oldestEmployee);
            Assert.Equal(employees.OrderByDescending(c => c.BirthDay).First(), oldestEmployee);
        }

        [Fact]
        public void GetAverageAge_PositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(10);
            _employeeStorage.Add(employees);

            var averageAge = _employeeStorage.GetAverageAgeEmployee();
            var expectedAverageAge = employees
                .Select(e => DateTime.Now.Year - e.BirthDay.Year - (DateTime.Now.DayOfYear < e.BirthDay.DayOfYear ? 1 : 0))
                .Average();

            Assert.Equal(expectedAverageAge, averageAge, 1);
        }

        [Fact]
        public void UpdateEmployee_PositiveTest()
        {
            // Arrange: создаем и добавляем сотрудника
            var employee = new Employee
            {
                FirstName = "Иван",
                LastName = "Иванов",
                PhoneNumber = "1234567890",
                Position = "Менеджер",
                BirthDay = new DateTime(1990, 1, 1),
                Contract = "Договор 1",
                Salary = 50000
            };

            _employeeStorage.Add(employee);

            // Создаем обновленного сотрудника
            var updatedEmployee = new Employee
            {
                FirstName = employee.FirstName,
                LastName = "Обновленный", // изменяем фамилию для обновления
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                BirthDay = employee.BirthDay,
                Contract = employee.Contract,
                Salary = employee.Salary
            };

            // Act: обновляем сотрудника
            _employeeStorage.Update(updatedEmployee);

            // Assert: проверяем, что сотрудник был обновлен
            var employees = _employeeStorage.Get(e => true);
            Assert.Equal(updatedEmployee.LastName, employees[0].LastName);
            Assert.Equal(updatedEmployee.PhoneNumber, employees[0].PhoneNumber);
        }

        [Fact]
        public void UpdateEmployee_EmployeeNotFound_ThrowsInvalidOperationException()
        {
            var employee = new Employee(); 
            
            Assert.Throws<InvalidOperationException>(() => _employeeStorage.Update(employee));
        }

        [Fact]
        public void RemoveEmployees_PositiveTest()
        {
            var employees = _dataGenerator.GenerateEmployees(3);
            _employeeStorage.Add(employees);
            
            _employeeStorage.Delete(employees.First());

            Assert.Equal(2, _employeeStorage.Get(e => true).Count);
        }

        [Fact]
        public void RemoveEmployee_EmployeeNotFound_ThrowsInvalidOperationException()
        {
            var employee = new Employee(); 
            
            Assert.Throws<InvalidOperationException>(() => _employeeStorage.Delete(employee));
        }
    }
}
