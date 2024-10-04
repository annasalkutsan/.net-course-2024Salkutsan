namespace BankSystem.App.Exceptions;

public class PositionException : EmployeeException
{
    public PositionException() 
        : base("Должность сотрудника не может быть пустой или содержать только пробелы.") 
    { }
}