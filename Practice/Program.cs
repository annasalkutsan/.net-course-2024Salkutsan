using System.Diagnostics;
using System.Diagnostics.Contracts;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace Practice
{
    public class Program
    {
        static void Main(string[] args)
        { 
            /*TestDataGenerator generator = new TestDataGenerator();
        
            // Генерация данных
            List<Client> clients = generator.GenerateClients(1000);
            Dictionary<string, Client> clientDictionary = generator.GenerateClientDictionary(clients);
            List<Employee> employees = generator.GenerateEmployees(1000);

            string searchPhoneNumber = clients[500].PhoneNumber; // Пример номера телефона для поиска

            // Измерение времени поиска клиента по номеру телефона в списке
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); 
            var clientFromList = clients.FirstOrDefault(c => c.PhoneNumber == searchPhoneNumber);
            stopwatch.Stop(); 
            Console.WriteLine($"Поиск клиента по номеру телефона в списке занял: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");

            // Измерение времени поиска клиента по номеру телефона в словаре
            stopwatch.Restart();
            var clientFromDictionary = clientDictionary.GetValueOrDefault(searchPhoneNumber);
            stopwatch.Stop();
            Console.WriteLine($"Поиск клиента по номеру телефона в словаре занял: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");

            // Выборка клиентов, возраст которых ниже определенного значения
            var ageLimit = DateTime.Now.AddYears(-30);
            var youngClients = clients.Where(c => c.BirthDay > ageLimit).ToList();
            Console.WriteLine($"Количество клиентов младше 30 лет: {youngClients.Count}");

            // Поиск сотрудника с минимальной заработной платой
            var minSalaryEmployee = employees.OrderBy(e => e.Salary).FirstOrDefault();
            Console.WriteLine($"Сотрудник с минимальной зарплатой: {minSalaryEmployee.FirstName} {minSalaryEmployee.LastName}, Зарплата: {minSalaryEmployee.Salary}");

            // Сравнение скорости поиска по словарю
            // 1. При помощи метода LastOrDefault
            stopwatch.Restart();
            var lastClientInList = clients.LastOrDefault();
            stopwatch.Stop(); 
            Console.WriteLine($"Поиск последнего элемента в списке занял: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");

            // 2. Поиск в словаре по ключу
            stopwatch.Restart(); 
            var clientFromDictByKey = clientDictionary.GetValueOrDefault(lastClientInList.PhoneNumber);
            stopwatch.Stop(); 
            Console.WriteLine($"Поиск по ключу в словаре занял: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");
        }
        
        static void UpdateEmployeeContract(Employee employee)
        {
            employee.Contract = $"Контракт для {employee.FirstName} {employee.LastName}; Должность: {employee.Position}; Контактный номер: {employee.PhoneNumber}";
        }
        
        static void UpdateCurrency(ref Currency currency, string newCode, string newName)
        {
            currency.Code = newCode;
            currency.Name = newName;
        }

        static void Practice1()
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
        }

        static void Practice2()
        {
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
            Console.WriteLine($"Новый сотрудник: {newEmployee.FirstName} {newEmployee.LastName}, Должность: {newEmployee.Position}, Контракт: {newEmployee.Contract}");*/
        }
    }
}
