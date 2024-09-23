using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    /// <summary>
    /// Класс "Клиент"
    /// </summary>
    public class Client:Person
    {
        //поле серии и номера паспорта объединила просто в Passport
        
        /// <summary>
        /// Данные паспорта
        /// </summary>
        public string Passport { get; set; }
        
        /// <summary>
        /// Дата регистрации в банке
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        //дополнила конструктор Client
        public Client(string passport, DateTime registrationDate, string firstName, string lastName, string phoneNumber, DateTime birthDay)
            : base(firstName, lastName, phoneNumber, birthDay)
        {
            Passport = passport;
            RegistrationDate = registrationDate;
        }
        
        public Client() { }
    }
}
