using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Helpers.Consulta;
using Domain.Helpers.Login;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackaMed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginUseCase _loginUseCase;

        public LoginController(ILogger<LoginController> logger, ILoginUseCase loginUseCase)
        {
            _logger = logger;
            _loginUseCase = loginUseCase;
        }

        [HttpPost]
        public async Task<IResult> Login(LoginInput loginInput)
        {
            try
            {
                if (loginInput is null)
                    return TypedResults.BadRequest("Dados de login inválidos");

                var usuario = await _loginUseCase.Login(loginInput);
                return TypedResults.Ok(usuario);
            }
            catch (ValidationException ex)
            {
                var error = "Erro ao realizar login";

                _logger.LogError(error, ex);
                return TypedResults.BadRequest(error);
            }
            catch (Exception ex)
            {
                var erro = "Erro ao realizar login";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }
    }
}
