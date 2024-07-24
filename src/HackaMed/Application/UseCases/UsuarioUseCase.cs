using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Helpers;
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

        public async Task<IList<Usuario>> GetAllUsuarios()
        {
            try
            {
                return await _usuarioRepository.GetAllUsuarios();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> GetUsuarioById(string id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetUsuarioById(id);
                if (usuario == null) return new Usuario();

                return usuario;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUsuario(string id, Usuario usuario)
        {
            try
            {
                var usuarioOriginal = await _usuarioRepository.GetUsuarioById(id);
                if (usuarioOriginal is null) throw new Exception("Usuario não encontrado");

                usuarioOriginal.Nome = usuario.Nome;
                usuarioOriginal.Email = usuario.Email;
                usuarioOriginal.CPF = usuario.CPF;

                await _usuarioRepository.UpdateUsuario(id, usuarioOriginal);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteUsuario(string id)
        {
            try
            {
                var usuarioOriginal = await _usuarioRepository.GetUsuarioById(id);
                if (usuarioOriginal is null) throw new Exception("Usuario não encontrado");

                await _usuarioRepository.DeleteUsuario(id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
