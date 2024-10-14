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
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        
        public Client(string passport, string firstName, string lastName, string phoneNumber, DateTime birthDay)
            : base(firstName, lastName, phoneNumber, birthDay)
        {
            Passport = passport;
        }
        
        public Client() { }
        
        public override bool Equals(object obj)
        {
            if (obj is Client otherClient)
            {
                return Passport == otherClient.Passport;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Passport);
        }
    }
}
