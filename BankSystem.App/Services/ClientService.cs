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

        public Client GetClient(Guid id)
        {
            var client = _clientStorage.Get(id);
            if (client == null)
            {
                throw new KeyNotFoundException("Клиент не найден.");
            }
            return client;
        }

        public ICollection<Client> GetAllClients()
        {
            return _clientStorage.GetAll();
        }

        public void AddClient(Client client)
        {
            ValidateClient(client); 
            _clientStorage.Add(client);
        }

        public void UpdateClient(Guid clientId, Client updatedClient)
        {
            if (clientId != updatedClient.Id)
            {
                throw new ArgumentException("Идентификатор клиента не совпадает с идентификатором обновлённого клиента.");
            }
            
            _clientStorage.Update(updatedClient);
        }


        public void DeleteClient(Guid clientId)
        {
            // временнsq объект Client для удаления
            var clientToDelete = new Client { Id = clientId };

            _clientStorage.Delete(clientToDelete);
        }


        public ICollection<Client> GetClientsByFilter(
            string lastName = null, 
            string phoneNumber = null, 
            string passport = null, 
            DateTime? birthStart = null, 
            DateTime? birthEnd = null,
            int pageNumber = 1, // номер страницы
            int pageSize = 10)  // количество записей на странице
        {
            // все клиенты по фильтру
            var query = _clientStorage.GetByFilter(c => 
                (string.IsNullOrEmpty(lastName) || c.LastName == lastName) &&
                (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber == phoneNumber) &&
                (string.IsNullOrEmpty(passport) || c.Passport == passport) &&
                (!birthStart.HasValue || c.BirthDay >= birthStart.Value) &&
                (!birthEnd.HasValue || c.BirthDay <= birthEnd.Value));
    
            // пагинация
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public ICollection<Account> GetAccountsByClientId(Guid clientId)
        {
            return _clientStorage.GetAccountsByClientId(clientId);
        }

        public void AddAccount(Guid clientId, Account account)
        {
            _clientStorage.AddAccount(clientId, account);
        }

        public void UpdateAccount(Account account)
        {
            _clientStorage.UpdateAccount(account);
        }

        public void DeleteAccount(Guid accountId)
        {
            _clientStorage.DeleteAccount(accountId);
        }
        
        private void ValidateClient(Client client)
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

            if (_clientStorage.GetAll().Any(c => c.Passport == client.Passport && c.Id != client.Id))
            {
                throw new InvalidOperationException("Клиент с таким паспортом уже существует.");
            }
        }
    }
}