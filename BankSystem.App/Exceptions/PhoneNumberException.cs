namespace BankSystem.App.Exceptions;

public class PhoneNumberException : EmployeeException
{
    public PhoneNumberException() 
        : base("Номер телефона не может быть пустым или содержать только пробелы.") 
    { }
}