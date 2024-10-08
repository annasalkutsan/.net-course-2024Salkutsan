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
        public void Add_ShouldAddEmployee()
        {
            // Arrange
            var employee = new Employee("Manager", "Permanent", 3000, "John", "Doe", "+373 777 66 100", new DateTime(1990, 1, 1));

            // Act
            _employeeStorage.Add(employee);
            var result = _employeeStorage.Get(e => e.PhoneNumber == employee.PhoneNumber);

            // Assert
            Assert.Single(result);
            Assert.Equal(employee.FirstName, result[0].FirstName);
        }

        [Fact]
        public void AddCollection_ShouldAddMultipleEmployees()
        {
            // Arrange
            var employees = _dataGenerator.GenerateEmployees(5);

            // Act
            _employeeStorage.AddCollection(employees);
            var result = _employeeStorage.Get(e => true);

            // Assert
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void Update_ShouldModifyEmployee()
        {
            // Arrange
            var employee = new Employee("Manager", "Permanent", 3000, "John", "Doe", "+373 777 66 100", new DateTime(1990, 1, 1));
            _employeeStorage.Add(employee);
            var updatedEmployee = new Employee("Senior Manager", "Permanent", 4000, "John", "Doe", "+373 777 66 100", new DateTime(1985, 1, 1));

            // Act
            _employeeStorage.Update(updatedEmployee);
            var result = _employeeStorage.Get(e => e.PhoneNumber == employee.PhoneNumber);

            // Assert
            Assert.Single(result);
            Assert.Equal(updatedEmployee.Position, result[0].Position);
            Assert.Equal(updatedEmployee.Salary, result[0].Salary);
            Assert.Equal(updatedEmployee.BirthDay, result[0].BirthDay);
        }

        [Fact]
        public void Delete_ShouldRemoveEmployee()
        {
            // Arrange
            var employee = new Employee("Manager", "Permanent", 3000, "John", "Doe", "+373 777 66 100", new DateTime(1990, 1, 1));
            _employeeStorage.Add(employee);

            // Act
            _employeeStorage.Delete(employee);
            var result = _employeeStorage.Get(e => e.PhoneNumber == employee.PhoneNumber);

            // Assert
            Assert.Empty(result);
        }

       /* [Fact]
        public void GetYoungestEmployee_ShouldReturnYoungest()
        {
            // Arrange
            var employees = _dataGenerator.GenerateEmployees(5);
            _employeeStorage.AddCollection(employees);

            // Act
            var youngest = _employeeStorage.GetYoungestEmployee();

            // Assert
            Assert.NotNull(youngest);
            Assert.Equal(employees.OrderBy(e => e.BirthDay).Last().PhoneNumber, youngest.PhoneNumber);
        }

        [Fact]
        public void GetOldestEmployee_ShouldReturnOldest()
        {
            // Arrange
            var employees = _dataGenerator.GenerateEmployees(5);
            _employeeStorage.AddCollection(employees);

            // Act
            var oldest = _employeeStorage.GetOldestEmployee();

            // Assert
            Assert.NotNull(oldest);
            Assert.Equal(employees.OrderBy(e => e.BirthDay).First().PhoneNumber, oldest.PhoneNumber);
        }*/

        [Fact]
        public void GetAverageAgeEmployee_ShouldReturnCorrectAverageAge()
        {
            // Arrange
            var employees = _dataGenerator.GenerateEmployees(5);
            _employeeStorage.AddCollection(employees);

            // Act
            var averageAge = _employeeStorage.GetAverageAgeEmployee();

            // Assert
            Assert.True(averageAge > 0);
        }
    }
}
