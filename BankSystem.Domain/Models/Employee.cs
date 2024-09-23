using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    /// <summary>
    /// Класс "Сотрудник"
    /// </summary>
    public class Employee:Person
    {
        /// <summary>
        /// Должность 
        /// </summary>
        public string Position { get; set; }
        
        /// <summary>
        /// Контракт 
        /// </summary>
        public string Contract {  get; set; }

        public Employee(string position, string contract)
        {
            Position = position;
            Contract = contract;
        }
        
        public Employee () { }
    }
}
