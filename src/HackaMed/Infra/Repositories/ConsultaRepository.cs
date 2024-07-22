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
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly IMongoCollection<Consulta> _collection;

        public ConsultaRepository(IDatabaseConfig databaseConfig)
        {
            var connectionString = databaseConfig.ConnectionString.Replace("user", databaseConfig.User).Replace("password", databaseConfig.Password);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseConfig.DatabaseName);
            _collection = database.GetCollection<Consulta>("Consulta");
        }

        public async Task<Consulta> CreateConsulta(Consulta consulta)
        {
            await _collection.InsertOneAsync(consulta);
            return consulta;
        }

        public async Task<IList<Consulta>> GetAllConsultas()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Consulta> GetConsultaById(string id)
        {
            return await _collection.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
        }

        public async Task UpdateConsulta(string id, Consulta usuario)
        {
            await _collection.ReplaceOneAsync(x => x.Id.ToString() == id, usuario);
        }

        public async Task DeleteConsulta(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
