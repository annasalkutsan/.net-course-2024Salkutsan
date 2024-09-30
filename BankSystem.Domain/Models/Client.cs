using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Client:Person
    {
        public string Passport { get; set; }
        public DateTime RegistrationDate { get; set; }
        
        public Client(string passport, DateTime registrationDate, string firstName, string lastName, string phoneNumber, DateTime birthDay)
            : base(firstName, lastName, phoneNumber, birthDay)
        {
            Passport = passport;
            RegistrationDate = registrationDate;
        }
        
        public Client() { }
        
        public override bool Equals(object obj)
        {
            if (obj is Client otherClient)
            {
                return Passport == otherClient.Passport &&
                       FirstName == otherClient.FirstName &&
                       LastName == otherClient.LastName &&
                       PhoneNumber == otherClient.PhoneNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Passport, FirstName, LastName, PhoneNumber);
        }
    }
}
