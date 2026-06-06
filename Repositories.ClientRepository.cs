using MusicStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusicStore.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private List<Client> _clients = new()
        {
            new Client { Id = 1, FullName = "Иванов Иван Иванович", Phone = "+7 (999) 123-45-67", Email = "ivanov@mail.ru" },
            new Client { Id = 2, FullName = "Петров Пётр Петрович", Phone = "+7 (999) 765-43-21", Email = "petrov@gmail.com" }
        };

        public List<Client> GetAll() => _clients;
        public Client? GetById(int id) => _clients.FirstOrDefault(c => c.Id == id);
        public void Add(Client client)
        {
            client.Id = _clients.Max(c => c.Id) + 1;
            _clients.Add(client);
        }
        public void Update(Client client)
        {
            var existing = GetById(client.Id);
            if (existing != null)
            {
                existing.FullName = client.FullName;
                existing.Phone = client.Phone;
                existing.Email = client.Email;
            }
        }
        public void Delete(int id)
        {
            var client = GetById(id);
            if (client != null) _clients.Remove(client);
        }
    }
}