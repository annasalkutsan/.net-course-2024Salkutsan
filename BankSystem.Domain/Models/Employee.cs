namespace BankSystem.Domain.Models
{
    public class Employee:Person
    {
        public string Contract { get; set; }
        public decimal Salary { get; set; } 

        public Guid? PositionId { get; set; }
        public PositionStorage PositionStorage { get; set; }
        
        public Employee(string contract, decimal salary, string firstName, string lastName, string phoneNumber, DateTime birthDay)
            : base(firstName, lastName, phoneNumber, birthDay)
        {
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
