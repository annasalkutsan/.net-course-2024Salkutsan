using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Currency(string code, string name)
        {
            Code = code;
            Name = name;
        }
        public static bool operator ==(Currency left, Currency right)
        {
            return left.Code == right.Code;
        }

        public static bool operator !=(Currency left, Currency right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Currency other)
            {
                return Code == other.Code && Name == other.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode(); 
        }
    }
}
