using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Helpers.Consulta;
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
    public class ConsultaUseCase : IConsultaUseCase
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaRepository _agendaRepository;
        private readonly ILogger<ConsultaUseCase> _log;

        public ConsultaUseCase(IConsultaRepository consultaRepository, IAgendaRepository agendaRepository, ILogger<ConsultaUseCase> log)
        {
            _consultaRepository = consultaRepository;
            _agendaRepository = agendaRepository;
            _log = log;
        }

        public async Task<Consulta> CreateConsulta(ConsultaInput consulta)
        {
            try
            {
                var agenda = await _agendaRepository.GetAgendaById(consulta.AgendamentoId);

                var newConsulta = new Consulta()
                {
                    MedicoId = consulta.MedicoId,
                    PacienteId = consulta.PacienteId,
                    DataConsulta = agenda.DataAgendamento,
                    HorarioConsulta = agenda.HorarioAgendamento,
                    Valor = agenda.Valor,
                    DataCadastro = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    Status = "Pendente"
                };

                return await _consultaRepository.CreateConsulta(newConsulta);
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

        public async Task<IList<Consulta>> GetAllConsultas()
        {
            try
            {
                return await _consultaRepository.GetAllConsultas();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Consulta> GetConsultaById(string id)
        {
            try
            {
                var consulta = await _consultaRepository.GetConsultaById(id);
                if (consulta == null) return new Consulta();

                return consulta;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateConsulta(string id, Consulta consulta)
        {
            try
            {
                var consultaOriginal = await _consultaRepository.GetConsultaById(id);
                if (consultaOriginal is null) throw new Exception("Usuario não encontrado");

                consultaOriginal.Status = consulta.Status;
                consultaOriginal.Link = consulta.Link;
                consultaOriginal.DataAlteracao = DateTime.Now;

                await _consultaRepository.UpdateConsulta(id, consultaOriginal);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteConsulta(string id)
        {
            try
            {
                var agendaOriginal = await _consultaRepository.GetConsultaById(id);
                if (agendaOriginal is null) throw new Exception("Consulta não encontrado");

                await _consultaRepository.DeleteConsulta(id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateConsultaStatus(string id, string status, string descricao)
        {
            try
            {
                var consultaOriginal = await _consultaRepository.GetConsultaById(id);
                if (consultaOriginal is null) throw new Exception("Usuario não encontrado");

                if (consultaOriginal.Status == "Cancelada" && status == "Aprovada")
                    throw new Exception("Não é possível ativar uma consulta cancelada");

                //consultaOriginal.Link = ""; GERAR LINK PARA A REUNIÃO
                //Lembrar de deixar o status indisponivel da respectiva agenda ao aceitar a consulta

                consultaOriginal.Status = status;
                consultaOriginal.DescricaoCancelamento = descricao;
                consultaOriginal.DataAlteracao = DateTime.Now;

                await _consultaRepository.UpdateConsulta(id, consultaOriginal);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
