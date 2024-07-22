using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class UsuarioUseCase : IUsuarioUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioUseCase> _log;

        public UsuarioUseCase(IUsuarioRepository usuarioRepository, ILogger<UsuarioUseCase> log)
        {
            _usuarioRepository = usuarioRepository;
            _log = log;
        }

        public async Task<Usuario> CreateUsuario(Usuario usuario)
        {
            try
            {
                return await _usuarioRepository.CreateUsuario(usuario);
            }
            catch (ValidationException ex)
            {
                _log.LogError(ex.Message);
                throw new ValidationException(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
