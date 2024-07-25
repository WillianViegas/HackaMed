﻿using Domain.Entities;
using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUsuarioRepository
    {
        public Task<Usuario> CreateUsuario(Usuario usuario);
        public Task<IList<Usuario>> GetAllUsuarios();
        public Task<Usuario> GetUsuarioById(string id);
        public Task UpdateUsuario(string id, Usuario usuario);
        public Task DeleteUsuario(string id);
        public Task<IList<Usuario>> GetAllMedicos(MedicoFilter medicoFilter);
        public Task<Usuario> GetUsuarioByLogin(string usuario, string senha, string tipoIdentificacao);
    }
}