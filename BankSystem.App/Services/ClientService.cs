using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private readonly IClientStorage _clientStorage;

        public ClientService(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public async Task<Client> GetClientAsync(Guid id)
        {
            var client = await _clientStorage.GetAsync(id);
            if (client == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }
            return client;
        }

        public async Task<ICollection<Client>> GetAllClientsAsync()
        {
            return await _clientStorage.GetAllAsync();
        }

        public async Task AddClientAsync(Client client)
        {
            await ValidateClientAsync(client);
            await _clientStorage.AddAsync(client);
        }

        public async Task UpdateClientAsync(Guid clientId, Client updatedClient)
        {
            await _clientStorage.UpdateAsync(clientId, updatedClient);
        }

        public async Task DeleteClientAsync(Guid clientId)
        {
            await _clientStorage.DeleteAsync(clientId);
        }

        public async Task<ICollection<Client>> GetClientsByFilterAsync(
            string lastName = null, 
            string phoneNumber = null, 
            string passport = null, 
            DateTime? birthStart = null, 
            DateTime? birthEnd = null,
            int pageNumber = 1, // номер страницы
            int pageSize = 10)  // количество записей на странице
        {
            var clients = await _clientStorage.GetByFilterAsync(c => 
                (string.IsNullOrEmpty(lastName) || c.LastName == lastName) &&
                (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber == phoneNumber) &&
                (string.IsNullOrEmpty(passport) || c.Passport == passport) &&
                (!birthStart.HasValue || c.BirthDay >= birthStart.Value) &&
                (!birthEnd.HasValue || c.BirthDay <= birthEnd.Value));
    
            return clients.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<ICollection<Account>> GetAccountsByClientIdAsync(Guid clientId)
        {
            return await _clientStorage.GetAccountsByClientIdAsync(clientId);
        }

        public async Task AddAccountAsync(Guid clientId, Account account)
        {
            await _clientStorage.AddAccountAsync(clientId, account);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await _clientStorage.UpdateAccountAsync(account);
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            await _clientStorage.DeleteAccountAsync(accountId);
        }
        
        private async Task ValidateClientAsync(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Клиент не может быть нулевым.");
            }

            if (client.BirthDay > DateTime.Now.AddYears(-18))
            {
                throw new AgeException();
            }

            if (string.IsNullOrWhiteSpace(client.Passport))
            {
                throw new PassportException();
            }

            var existingClients = await _clientStorage.GetByFilterAsync(c => c.Passport == client.Passport && c.Id != client.Id);
            if (existingClients.Any())
            {
                throw new InvalidOperationException("Клиент с таким паспортом уже существует.");
            }
        }
    }
}