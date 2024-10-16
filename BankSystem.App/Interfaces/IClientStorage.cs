using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientStorage : IStorage<Client>
{
    ICollection<Account> GetAccountsByClientId(Guid clientId); 
    void AddAccount(Guid clientId, Account account); 
    void UpdateAccount(Account account); 
    void DeleteAccount(Guid accountId); 
}