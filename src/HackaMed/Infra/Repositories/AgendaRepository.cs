using Domain.Entities;
using Domain.Repositories;
using Infra.DatabaseConfig;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly IMongoCollection<Agenda> _collection;

        public AgendaRepository(IDatabaseConfig databaseConfig)
        {
            var connectionString = databaseConfig.ConnectionString.Replace("user", databaseConfig.User).Replace("password", databaseConfig.Password);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseConfig.DatabaseName);
            _collection = database.GetCollection<Agenda>("Agenda");
        }

        public async Task<Agenda> CreateAgenda(Agenda agenda)
        {
            await _collection.InsertOneAsync(agenda);
            return agenda;
        }

        public async Task<Agenda> GetAgendaById(string id)
        {
            return await _collection.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Agenda>> GetAllAgendas()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAgenda(string id, Agenda agenda)
        {
            await _collection.ReplaceOneAsync(x => x.Id.ToString() == id, agenda);
        }

        public async Task DeleteAgenda(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IList<Agenda>> GetAgendaByMedicoId(string id)
        {
            return await _collection.Find(x => x.MedicoId.ToString() == id).ToListAsync();
        }
    }
}
