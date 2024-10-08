using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Employee:Person
    {
        public string Position { get; set; }
        public string Contract { get; set; }
        public decimal Salary { get; set; } 

        public Employee(string position, string contract, decimal salary, string firstName, string lastName, string phoneNumber, DateTime birthDay)
            : base(firstName, lastName, phoneNumber, birthDay)
        {
            Position = position;
            Contract = contract;
            Salary = salary;
        }

        public Employee() { }
        
        public override bool Equals(object obj)
        {
            if (obj is Employee otherEmployee)
            {
                return PhoneNumber == otherEmployee.PhoneNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PhoneNumber);
        }
    }
}
