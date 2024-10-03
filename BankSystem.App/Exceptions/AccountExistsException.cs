namespace BankSystem.App.Exceptions;

public class AccountExistsException : ClientException
{
    public AccountExistsException() : base("Счет уже существует.") { }
}