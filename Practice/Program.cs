using System.Diagnostics.Contracts;
using BankSystem.Domain.Models;

namespace Practice
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Практическое задание 1 "Типы значений и ссылочные типы"
            Employee employee = new Employee
            {
                FirstName = "Степан",
                LastName = "Музыченко",
                PhoneNumber = "+373 775 21 441",
                BirthDay = new DateTime(1995, 5, 20),
                Position = "Менеджер"
            };
            Console.WriteLine($"Старый контракт: {employee.Contract}");
           
            UpdateEmployeeContract(employee);
            Console.WriteLine($"Обновленный контракт: {employee.Contract}");
            
            Currency currency = new Currency("USD", "United States Dollar");
            Console.WriteLine($"Старая валюта: Code = {currency.Code}, Name = {currency.Name}");
            
            UpdateCurrency(ref currency, "EUR", "Euro");
            Console.WriteLine($"Обновленная валюта: Code = {currency.Code}, Name = {currency.Name}");
            
        }

        /// <summary>
        /// Обновление контракта сотрудника на основе его данных
        /// </summary>
        static void UpdateEmployeeContract(Employee employee)
        {
            employee.Contract = $"Контракт для {employee.FirstName} {employee.LastName}; Должность: {employee.Position}; Контактный номер: {employee.PhoneNumber}";
        }

        /// <summary>
        /// Обновление свойств валюты
        /// </summary>
        static void UpdateCurrency(ref Currency currency, string newCode, string newName)
        {
            currency.Code = newCode;
            currency.Name = newName;
        }
    }
}
