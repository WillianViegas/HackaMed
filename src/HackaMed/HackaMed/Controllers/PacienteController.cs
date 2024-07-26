using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackaMed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PacienteController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioUseCase _usuarioUseCase;
        private readonly IPacienteUseCase _pacienteUseCase;


        public PacienteController(ILogger<UsuarioController> logger, IUsuarioUseCase usuarioUseCase, IPacienteUseCase pacienteUseCase)
        {
            _logger = logger;
            _usuarioUseCase = usuarioUseCase;
            _pacienteUseCase = pacienteUseCase;
        }

        [HttpPost("GetAllMedicos/")]
        public async Task<IResult> GetAllMedicos(MedicoFilter medicoFilter)
        {
            try
            {
                var medicos = await _pacienteUseCase.GetAllMedicos(medicoFilter);
                if (!medicos.Any()) return TypedResults.NotFound("Usuário não encontrado");

                return TypedResults.Ok(medicos);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao buscar lista de médicos";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPost("CadastrarProntuario/")]
        public async Task<IResult> CadastrarProntuario(Prontuario prontuario)
        {
            try
            {
                prontuario = await _pacienteUseCase.CadastrarProntuario(prontuario);
                return TypedResults.Ok(prontuario);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message, ex);
                return TypedResults.Problem(ex.Message);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao cadastrar prontuário.";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPut("AdicionarDocumento/")]
        public async Task<IResult> AdicionarDocumento([FromForm] Documento documento, IFormFile file)
        {
            try
            {

                if (file == null || file.Length == 0)
                {
                    return TypedResults.BadRequest("Nenhum arquivo selecionado para upload.");
                }

                var prontuario = await _pacienteUseCase.AdicionarDocumento(documento, file);
                return TypedResults.Ok(prontuario);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message, ex);
                return TypedResults.Problem(ex.Message);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao adicionar documento ao prontuário.";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        [HttpPut("RemoverDocumento/")]
        public async Task<IResult> RemoverDocumento(Documento documento)
        {
            try
            {
                var prontuario = await _pacienteUseCase.RemoverDocumento(documento);
                return TypedResults.Ok(prontuario);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message, ex);
                return TypedResults.Problem(ex.Message);
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao remover documento do prontuário.";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }
    }
}
