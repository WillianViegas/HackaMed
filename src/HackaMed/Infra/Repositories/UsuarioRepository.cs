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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _collection;

        public UsuarioRepository(IDatabaseConfig databaseConfig)
        {
            var connectionString = databaseConfig.ConnectionString.Replace("user", databaseConfig.User).Replace("password", databaseConfig.Password);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseConfig.DatabaseName);
            _collection = database.GetCollection<Usuario>("Usuario");
        }

        public async Task<Usuario> CreateUsuario(Usuario usuario)
        {
            await _collection.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task<IList<Usuario>> GetAllUsuarios()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario> GetUsuarioById(string id)
        {
            return await _collection.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
        }

        public async Task UpdateUsuario(string id, Usuario usuario)
        {
            await _collection.ReplaceOneAsync(x => x.Id.ToString() == id, usuario);
        }

        public async Task DeleteUsuario(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
