using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Entities.DTO;
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
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                usuario.Senha = passwordHash;

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

        public async Task<IList<UsuarioDTO>> GetAllUsuarios()
        {
            try
            {
                var listaUsuarios = await _usuarioRepository.GetAllUsuarios();

                return listaUsuarios.Select(x => new UsuarioDTO()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    CPF = x.CPF,
                    Email = x.Email,
                    CRM = x.CRM,
                    Status = x.Status,
                    Especialidade = x.Especialidade,
                    Perfil = x.Perfil,
                    Endereco = x.Endereco,
                    DataCadastro = x.DataCadastro,
                    DataAlteracao = x.DataAlteracao,
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioDTO> GetUsuarioById(string id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetUsuarioById(id);
                if (usuario == null) return new UsuarioDTO();

                var usuarioDTO = new UsuarioDTO()
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    CPF = usuario.CPF,
                    Email = usuario.Email,
                    CRM = usuario.CRM,
                    Status = usuario.Status,
                    Especialidade = usuario.Especialidade,
                    Perfil = usuario.Perfil,
                    Endereco = usuario.Endereco,
                    DataCadastro = usuario.DataCadastro,
                    DataAlteracao = usuario.DataAlteracao
                };

                return usuarioDTO;
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
