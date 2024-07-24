using Application.UseCases;
using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Helpers.Consulta;
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
        public async Task<IResult> CreateConsulta(ConsultaInput consulta)
        {
            try
            {
                if (consulta is null)
                    return TypedResults.BadRequest("Dados da consulta inválidos");

                if (string.IsNullOrEmpty(consulta.PacienteId) || string.IsNullOrEmpty(consulta.MedicoId) || string.IsNullOrEmpty(consulta.AgendamentoId))
                    throw new ValidationException("Dados inválidos para agendamento de consulta");

                var newConsulta = await _consultaUseCase.CreateConsulta(consulta);
                return TypedResults.Created($"/consulta/{newConsulta.Id}", newConsulta);
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

        [HttpGet]
        public async Task<IResult> BuscarConsultas()
        {
            try
            {
                var consultas = await _consultaUseCase.GetAllConsultas();
                if (!consultas.Any()) return TypedResults.NotFound("Nenhum usuário encontrado");

                return TypedResults.Ok(consultas);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar consultas.";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpGet("GetConsultaById/{id}")]
        public async Task<IResult> GetConsultaById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");


                var usuario = await _consultaUseCase.GetConsultaById(id);
                if (usuario is null || string.IsNullOrEmpty(usuario.Id)) return TypedResults.NotFound("Consulta não encontrada");

                return TypedResults.Ok(usuario);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar consulta pelo id. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPut("UpdateConsulta/{id}")]
        public async Task<IResult> UpdateConsulta(string id, Consulta consulta)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var consultaOriginal = await _consultaUseCase.GetConsultaById(id);
                if (consultaOriginal is null || string.IsNullOrEmpty(consultaOriginal.Id)) return TypedResults.NotFound("Consulta não encontrado");

                await _consultaUseCase.UpdateConsulta(id, consulta);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao atualizar o consulta. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpDelete("DeleteConsulta/{id}")]
        public async Task<IResult> DeleteConsulta(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var consultaOriginal = await _consultaUseCase.GetConsultaById(id);
                if (consultaOriginal is null || string.IsNullOrEmpty(consultaOriginal.Id)) return TypedResults.NotFound("Consulta não encontrada");

                await _consultaUseCase.DeleteConsulta(id);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao deletar a consulta. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPut("CancelarConsulta/{id}")]
        public async Task<IResult> CancelarConsulta(string id, string descricao)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var consultaOriginal = await _consultaUseCase.GetConsultaById(id);
                if (consultaOriginal is null || string.IsNullOrEmpty(consultaOriginal.Id)) return TypedResults.NotFound("Consulta não encontrada");

                await _consultaUseCase.UpdateConsultaStatus(id, "Cancelada", descricao);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao atualizar o consulta. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPut("AprovarConsulta/{id}")]
        public async Task<IResult> AprovarConsulta(string id, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return TypedResults.BadRequest("Id inválido");

                var consultaOriginal = await _consultaUseCase.GetConsultaById(id);
                if (consultaOriginal is null || string.IsNullOrEmpty(consultaOriginal.Id)) return TypedResults.NotFound("Consulta não encontrada");

                await _consultaUseCase.UpdateConsultaStatus(id, "Aprovada");
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao atualizar o consulta. Id: {id}";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }
    }
}
