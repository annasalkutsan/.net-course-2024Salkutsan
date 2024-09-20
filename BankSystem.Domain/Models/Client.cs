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
        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string PassportSeries { get; set; }
        
        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string PassportNumber { get; set; }
        
        /// <summary>
        /// Дата регистрации в банке
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        public Client(string passportSeries, string passportNumber, DateTime registrationDate)
        {
            PassportSeries = passportSeries;
            PassportNumber = passportNumber;
            RegistrationDate = registrationDate;
        }
        
        public Client() { }
    }
}
