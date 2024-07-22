using Application.UseCases.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
    }
}
