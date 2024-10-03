namespace BankSystem.App.Exceptions;

public class AgeException : ClientException
{
    public AgeException() : base("Клиент должен быть старше 18 лет.") { }
}