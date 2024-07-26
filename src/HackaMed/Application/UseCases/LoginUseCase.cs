using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Helpers.Login;
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
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProntuarioRepository _prontuarioRepository;
        private readonly ILogger<LoginUseCase> _log;

        public LoginUseCase(IUsuarioRepository usuarioRepository, IProntuarioRepository prontuarioRepository, ILogger<LoginUseCase> log)
        {
            _usuarioRepository = usuarioRepository;
            _prontuarioRepository = prontuarioRepository;
            _log = log;
        }

        public async Task<LoginResponse> Login(LoginInput loginInput)
        {
            try
            {
                var usuario = "";
                var senha = "";
                var tipoIdentificacao = "";

                //validar se usuario é medico ou paciente
                if (loginInput.Perfil == EnumPerfil.Paciente.ToString())
                {
                    if((string.IsNullOrEmpty(loginInput.Email) && string.IsNullOrEmpty(loginInput.CPF)) || string.IsNullOrEmpty(loginInput.Senha))
                    {
                        throw new Exception("Dados inválidos");
                    }

                    usuario = !string.IsNullOrEmpty(loginInput.Email) ? loginInput.Email : loginInput.CPF;
                    tipoIdentificacao = !string.IsNullOrEmpty(loginInput.Email) ? "Email" : "CPF";
                    senha = loginInput.Senha;
                }

                if(loginInput.Perfil == EnumPerfil.Medico.ToString())
                {

                    if (string.IsNullOrEmpty(loginInput.CRM) || string.IsNullOrEmpty(loginInput.Senha))
                    {
                        throw new Exception("Dados inválidos");
                    }

                    usuario = loginInput.CRM;
                    senha = loginInput.Senha;
                    tipoIdentificacao = "CRM";
                }

                // fazer respectiva validacao
                var usuarioLogin = await _usuarioRepository.GetUsuarioByLogin(usuario, tipoIdentificacao);

                //retornar usuario autenticado
                if (usuarioLogin == null || String.IsNullOrEmpty(usuarioLogin.Id))
                    throw new ValidationException("Login inválido!");

               if(!BCrypt.Net.BCrypt.Verify(senha, usuarioLogin.Senha))
                    throw new ValidationException("Login inválido!");

                var loginReponse = new LoginResponse()
                {
                    UsuarioId = usuarioLogin.Id,
                    Nome = usuarioLogin.Nome,
                    Email = usuarioLogin.Email,
                    CPF = usuarioLogin.CPF,
                    CRM = usuarioLogin.CRM,
                    Perfil = usuarioLogin.Perfil,
                    Status = usuarioLogin.Status
                };

                return loginReponse;
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
