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

        public Dictionary<Client, List<Account>> GetClientsByFilter(
            string lastName = null, 
            string phoneNumber = null, 
            string passport = null, 
            DateTime? birthStart = null, 
            DateTime? birthEnd = null)
        {
            return _clientStorage.Get(c => 
                (string.IsNullOrEmpty(lastName) || c.LastName == lastName) &&
                (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber == phoneNumber) &&
                (string.IsNullOrEmpty(passport) || c.Passport == passport) &&
                (!birthStart.HasValue || c.BirthDay >= birthStart.Value) &&
                (!birthEnd.HasValue || c.BirthDay <= birthEnd.Value));
        }
        
        public void Add(Client client)
        {
            ValidateClient(client);
            _clientStorage.Add(client);
        }

        public void Add(Dictionary<Client, List<Account>> clients)
        {
            foreach (var client in clients)
            {
                ValidateClient(client.Key);
            }

            _clientStorage.Add(clients);
        }
        
        public void Update(Client updatedClient)
        {
            ValidateClient(updatedClient);
            _clientStorage.Update(updatedClient); 
        }

        public void Delete(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Клиент не может быть нулевым."); 
            }

            _clientStorage.Delete(client); 
        }

        public void AddAccount(Client client, Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
            }

            _clientStorage.AddAccount(client, account);
        }

        public void UpdateAccount(Client client, Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
            }

            _clientStorage.UpdateAccount(client, account); 
        }

        public void DeleteAccount(Client client, Account account)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Клиент не может быть нулевым."); 
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
            }

            _clientStorage.DeleteAccount(client, account); 
        }

        private void ValidateClient(Client client)
        {
            if (client.BirthDay > DateTime.Now.AddYears(-18))
            {
                throw new AgeException(); 
            }

            if (string.IsNullOrWhiteSpace(client.Passport))
            {
                throw new PassportException(); 
            }
        }
    }
}