using MusicStore.Models;
using System.Collections.Generic;

namespace MusicStore.Repositories
{
    public interface IClientRepository
    {
        List<Client> GetAll();
        Client? GetById(int id);
        void Add(Client client);
        void Update(Client client);
        void Delete(int id);
    }
}