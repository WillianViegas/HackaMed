using Application.UseCases.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Domain.Helpers;

namespace HackaMed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioUseCase _usuarioUseCase;


        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioUseCase usuarioUseCase)
        {
            _logger = logger;
            _usuarioUseCase = usuarioUseCase;
        }

        [HttpGet("/teste-usuario")]
        public IResult Teste()
        {
            try
            {
                return TypedResults.Ok("Teste");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        #region Usuário
        [HttpPost]
        public async Task<IResult> CreateUsuario(Usuario usuario)
        {
            try
            {
                if (usuario is null)
                    return TypedResults.BadRequest("Dados do usuário inválidos");

                usuario = await _usuarioUseCase.CreateUsuario(usuario);
                return TypedResults.Created($"/usuario/{usuario.Id}", usuario);
            }
            catch (ValidationException ex)
            {
                var error = "Erro ao criar usuário";

                _logger.LogError(error, ex);
                return TypedResults.BadRequest(error);
            }
            catch (Exception ex)
            {
                var erro = "Erro ao criar usuário";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpGet]
        public async Task<IResult> BuscarUsuarios()
        {
            try
            {
                var usuarios = await _usuarioUseCase.GetAllUsuarios();
                if (!usuarios.Any()) return TypedResults.NotFound("Nenhum usuário encontrado");

                return TypedResults.Ok(usuarios);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar usuários.";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpGet("GetUsuarioById/{id}")]
        public async Task<IResult> GetUsuarioById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");


                var usuario = await _usuarioUseCase.GetUsuarioById(id);
                if (usuario is null || string.IsNullOrEmpty(usuario.Id)) return TypedResults.NotFound("Usuário não encontrado");

                return TypedResults.Ok(usuario);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar usuário pelo id. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPut("UpdateUsuario/{id}")]
        public async Task<IResult> UpdateUsuario(string id, Usuario usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var usuarioOriginal = await _usuarioUseCase.GetUsuarioById(id);
                if (usuarioOriginal is null || string.IsNullOrEmpty(usuarioOriginal.Id)) return TypedResults.NotFound("Usuário não encontrado");

                await _usuarioUseCase.UpdateUsuario(id, usuario);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao atualizar o usuário. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpDelete("DeleteUsuario/{id}")]
        public async Task<IResult> DeleteUsuario(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var usuarioOriginal = await _usuarioUseCase.GetUsuarioById(id);
                if (usuarioOriginal is null || string.IsNullOrEmpty(usuarioOriginal.Id)) return TypedResults.NotFound("Usuário não encontrado");

                await _usuarioUseCase.DeleteUsuario(id);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao deletar a usuario. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }
        #endregion
    }
}
