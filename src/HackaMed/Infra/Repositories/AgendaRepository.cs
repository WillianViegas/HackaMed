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

        public async Task<Agenda> CreatePedido(Agenda agenda)
        {
            await _collection.InsertOneAsync(agenda);
            return agenda;
        }
    }
}
