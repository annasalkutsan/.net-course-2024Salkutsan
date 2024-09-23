using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    /// <summary>
    /// Класс "Человек"
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Имя 
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия 
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Номер телефона 
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDay { get; set; }

        public Person( string firstName, string lastName, string phoneNumber, DateTime birthDay)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDay = birthDay;
        }

        public Person() { }
    }
}
