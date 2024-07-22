using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Interfaces
{
    public interface IUsuarioUseCase
    {
        public Task<Usuario> CreateUsuario(Usuario usuario);
        public Task<IList<Usuario>> GetAllUsuarios();
        public Task<Usuario> GetUsuarioById(string id);
        public Task UpdateUsuario(string id, Usuario usuario);
        public Task DeleteUsuario(string id);
    }
}
