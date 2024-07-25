using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Enum;
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
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<ConsultaUseCase> _log;

        public ConsultaUseCase(IConsultaRepository consultaRepository, IAgendaRepository agendaRepository, IUsuarioRepository usuarioRepository,  ILogger<ConsultaUseCase> log)
        {
            _consultaRepository = consultaRepository;
            _agendaRepository = agendaRepository;
            _usuarioRepository = usuarioRepository;
            _log = log;
        }

        public async Task<Consulta> CreateConsulta(ConsultaInput consulta)
        {
            try
            {
                var agenda = await _agendaRepository.GetAgendaById(consulta.AgendamentoId);

                if (agenda == null)
                    throw new ValidationException("Agenda não encontrada para marcar a consulta.");

                if (agenda.Status == EnumAgenda.Indisponível.ToString())
                    throw new ValidationException("Agenda não disponível para marcar a consulta.");

                var medico = await _usuarioRepository.GetUsuarioById(consulta.MedicoId);

                if(medico == null)
                    throw new ValidationException("Médico responsável pelo agendamento não encontrado.");

                var consultas = await _consultaRepository.GetAllConsultas();

                if (consultas.Any(x => x.PacienteId == consulta.PacienteId && x.AgendaId == consulta.AgendamentoId))
                    throw new ValidationException("Você já solicitou uma consulta para essa data");

                var newConsulta = new Consulta()
                {
                    MedicoId = consulta.MedicoId,
                    PacienteId = consulta.PacienteId,
                    AgendaId = consulta.AgendamentoId,
                    DataConsulta = (DateOnly)agenda.DataAgendamento,
                    HorarioConsulta = agenda.HorarioAgendamento,
                    Valor = (decimal)agenda.Valor,
                    DataCadastro = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                    Status = EnumConsulta.Pendente.ToString(),
                    Teleconsulta = consulta.Teleconsulta,
                    EnderecoConsulta = consulta.Teleconsulta ? null : medico.Endereco
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
                if (consultaOriginal is null) throw new ValidationException("Usuario não encontrado");

                if ((consultaOriginal.Status == EnumConsulta.Cancelada.ToString() || consultaOriginal.Status == EnumConsulta.Recusada.ToString()) && status == EnumConsulta.Aprovada.ToString())
                    throw new ValidationException("Não é possível aprovar uma consulta cancelada ou recusada");

             
                consultaOriginal.Status = status;
                consultaOriginal.DescricaoCancelamento = descricao;
                consultaOriginal.DataAlteracao = DateTime.Now;

                var agenda = await _agendaRepository.GetAgendaById(consultaOriginal.AgendaId);
                if (agenda is null) throw new ValidationException("Agenda não encontrada");

                if (status == EnumConsulta.Aprovada.ToString())
                {
                    var baseUrl = "https://meet.jit.si/";
                    var meetingId = Guid.NewGuid().ToString();
                    var meetingUrl = baseUrl + meetingId;
                    consultaOriginal.Link = meetingUrl;

                    agenda.Status = EnumAgenda.Indisponível.ToString();
                   await _agendaRepository.UpdateAgenda(consultaOriginal.AgendaId, agenda);
                }
                else
                {
                    if(agenda.Status == EnumAgenda.Indisponível.ToString())
                    {
                        agenda.Status = EnumAgenda.Disponível.ToString();
                        await _agendaRepository.UpdateAgenda(consultaOriginal.AgendaId, agenda);
                    }
                }

                await _consultaRepository.UpdateConsulta(id, consultaOriginal);
            }
            catch(ValidationException ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
