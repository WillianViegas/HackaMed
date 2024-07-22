using Application.UseCases.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackaMed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly ILogger<AgendaController> _logger;
        private readonly IAgendaUseCase _agendaUseCase;

        public AgendaController(ILogger<AgendaController> logger, IAgendaUseCase agendaUseCase)
        {
            _logger = logger;
            _agendaUseCase = agendaUseCase;
        }

        [HttpGet("/teste-agenda")]
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
        public async Task<IResult> CreateAgenda(Agenda agenda)
        {
            try
            {
                if (agenda is null)
                    return TypedResults.BadRequest("Dados da agenda inválidos");

                agenda = await _agendaUseCase.CreateAgenda(agenda);
                return TypedResults.Created($"/usuario/{agenda.Id}", agenda);
            }
            catch (ValidationException ex)
            {
                var error = "Erro ao criar agenda";

                _logger.LogError(error, ex);
                return TypedResults.BadRequest(error);
            }
            catch (Exception ex)
            {
                var erro = "Erro ao criar agenda";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }
    }
}
