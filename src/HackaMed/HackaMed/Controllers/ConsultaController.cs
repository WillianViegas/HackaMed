using Application.UseCases.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackaMed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : ControllerBase
    {
        private readonly ILogger<ConsultaController> _logger;
        private readonly IConsultaUseCase _consultaUseCase;

        public ConsultaController(ILogger<ConsultaController> logger, IConsultaUseCase consultaUseCase)
        {
            _logger = logger;
            _consultaUseCase = consultaUseCase;
        }

        [HttpGet("/teste-consulta")]
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
        public async Task<IResult> CreateUsuario(Consulta consulta)
        {
            try
            {
                if (consulta is null)
                    return TypedResults.BadRequest("Dados da consulta inválidos");

                consulta = await _consultaUseCase.CreateConsulta(consulta);
                return TypedResults.Created($"/usuario/{consulta.Id}", consulta);
            }
            catch (ValidationException ex)
            {
                var error = "Erro ao criar consulta";

                _logger.LogError(error, ex);
                return TypedResults.BadRequest(error);
            }
            catch (Exception ex)
            {
                var erro = "Erro ao criar consulta";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }
    }
}
