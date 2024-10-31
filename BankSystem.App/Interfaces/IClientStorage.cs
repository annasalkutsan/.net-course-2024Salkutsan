using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientStorage : IStorage<Client>
{
    Task<ICollection<Account>> GetAccountsByClientIdAsync(Guid clientId);
    Task AddAccountAsync(Guid clientId, Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(Guid accountId);
    Task<ICollection<Account>> GetAllAccountsAsync();
    Task<Account> GetAccountByIdAsync(Guid accountId);
}