using Domain.Entities;
using Domain.Entities.DTO;
using Domain.Helpers;
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
        public Task<IList<UsuarioDTO>> GetAllUsuarios();
        public Task<UsuarioDTO> GetUsuarioById(string id);
        public Task UpdateUsuario(string id, Usuario usuario);
        public Task DeleteUsuario(string id);
    }
}
