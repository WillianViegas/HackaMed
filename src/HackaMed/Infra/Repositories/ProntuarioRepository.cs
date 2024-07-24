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
    public class ProntuarioRepository : IProntuarioRepository
    {
        private readonly IMongoCollection<Prontuario> _collection;

        public ProntuarioRepository(IDatabaseConfig databaseConfig)
        {
            var connectionString = databaseConfig.ConnectionString.Replace("user", databaseConfig.User).Replace("password", databaseConfig.Password);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseConfig.DatabaseName);
            _collection = database.GetCollection<Prontuario>("Prontuario");
        }

        public async Task<Prontuario> CreateProntuario(Prontuario prontuario)
        {
            await _collection.InsertOneAsync(prontuario);
            return prontuario;
        }

        public async Task<Prontuario> GetProntuarioByPacienteId(string id)
        {
            return await _collection.Find(x => x.PacienteId.ToString() == id).FirstOrDefaultAsync();
        }

        public async Task UpdateProntuario(string idProntuario, Prontuario prontuario)
        {
            await _collection.ReplaceOneAsync(x => x.Id.ToString() == idProntuario, prontuario);
        }
    }
}
