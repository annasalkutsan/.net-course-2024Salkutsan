namespace BankSystem.App.Exceptions;

public class AccountEditException : ClientException
{
    public AccountEditException() : base("Можно редактировать только количество средств на счете.") { }
}