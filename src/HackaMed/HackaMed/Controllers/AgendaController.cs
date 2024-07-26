using Application.UseCases;
using Application.UseCases.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackaMed.Controllers
{
    [Authorize]
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

        [HttpPost]
        public async Task<IResult> CreateAgenda(Agenda agenda)
        {
            try
            {
                if (agenda is null)
                    return TypedResults.BadRequest("Dados da agenda inválidos");

                agenda = await _agendaUseCase.CreateAgenda(agenda);
                return TypedResults.Created($"/agenda/{agenda.Id}", agenda);
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

        [HttpGet]
        public async Task<IResult> BuscarAgendas()
        {
            try
            {
                var agendas = await _agendaUseCase.GetAllAgendas();
                if (!agendas.Any()) return TypedResults.NotFound("Nenhuma agenda encontrado");

                return TypedResults.Ok(agendas);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar agendas.";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpGet("GetAgendaById/{id}")]
        public async Task<IResult> GetAgendaById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");


                var agenda = await _agendaUseCase.GetAgendaById(id);
                if (agenda is null || string.IsNullOrEmpty(agenda.Id)) return TypedResults.NotFound("Agenda não encontrada");

                return TypedResults.Ok(agenda);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar agenda pelo id. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPut("UpdateAgenda/{id}")]
        public async Task<IResult> UpdateAgenda(string id, Agenda agenda)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var agendaOriginal = await _agendaUseCase.GetAgendaById(id);
                if (agendaOriginal is null || string.IsNullOrEmpty(agendaOriginal.Id)) return TypedResults.NotFound("Agenda não encontrada");

                await _agendaUseCase.UpdateAgenda(id, agenda);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao atualizar a agenda. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpDelete("DeleteAgenda/{id}")]
        public async Task<IResult> DeleteAgenda(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var agendaOriginal = await _agendaUseCase.GetAgendaById(id);
                if (agendaOriginal is null || string.IsNullOrEmpty(agendaOriginal.Id)) return TypedResults.NotFound("Agenda não encontrada");

                await _agendaUseCase.DeleteAgenda(id);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao deletar a agenda. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpGet("GetAgendaByMedicoId/{id}")]
        public async Task<IResult> GetAgendaByMedicoId(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");


                var agenda = await _agendaUseCase.GetAgendaByMedicoId(id);
                if (!agenda.Any()) return TypedResults.NotFound("Agenda não encontrada");

                return TypedResults.Ok(agenda);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar agenda pelo id. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }
    }
}
