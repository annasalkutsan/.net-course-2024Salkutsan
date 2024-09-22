using System.Diagnostics.Contracts;
using BankSystem.App.Services;
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
                Position = "Сотрудник"
            };
            Console.WriteLine($"Старый контракт: {employee.Contract}");
           
            UpdateEmployeeContract(employee);
            Console.WriteLine($"Обновленный контракт: {employee.Contract}");
            
            Currency currency = new Currency("USD", "United States Dollar");
            Console.WriteLine($"Старая валюта: Code = {currency.Code}, Name = {currency.Name}");
            
            UpdateCurrency(ref currency, "EUR", "Euro");
            Console.WriteLine($"Обновленная валюта: Code = {currency.Code}, Name = {currency.Name}");
          
            //Практическое задание 2 "Приведение и преобразование типов"
            List<Person> owners = new List<Person>
            {
                new Employee { FirstName = "Кирилл", LastName = "Завтуров", Position = "Владелец" },
                new Employee { FirstName = "Евгений", LastName = "Гусенко", Position = "Владелец" }
            };
            int bankProfit = 1000000;
            int bankExpenses = 500000;
            
            BankService bankService = new BankService();
            
            int ownerSalary = bankService.CalculateOwnerSalary(bankProfit, bankExpenses, owners);
            Console.WriteLine($"Зарплата каждого владельца: {ownerSalary}");
            
            Client client = new Client { FirstName = "Михаил", LastName = "Ротарь", PhoneNumber = "+373 778 547 96", BirthDay = new DateTime(1991, 11, 11) };
            Employee newEmployee = bankService.ConvertClientToEmployee(client, "Менеджер");
            Console.WriteLine($"Новый сотрудник: {newEmployee.FirstName} {newEmployee.LastName}, Должность: {newEmployee.Position}, Контракт: {newEmployee.Contract}");
        }

        /// <summary>
        /// Обновление контракта сотрудника на основе его данных
        /// </summary>
        /// <param name="employee"></param>
        static void UpdateEmployeeContract(Employee employee)
        {
            employee.Contract = $"Контракт для {employee.FirstName} {employee.LastName}; Должность: {employee.Position}; Контактный номер: {employee.PhoneNumber}";
        }

        /// <summary>
        /// Обновление свойств валюты
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="newCode"></param>
        /// <param name="newName"></param>
        static void UpdateCurrency(ref Currency currency, string newCode, string newName)
        {
            currency.Code = newCode;
            currency.Name = newName;
        }
    }
}
