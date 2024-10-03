namespace BankSystem.App.Exceptions;

public class PassportException : ClientException
{
    public PassportException() : base("У клиента отсутствуют паспортные данные.") { }
}