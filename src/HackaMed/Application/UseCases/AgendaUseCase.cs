using Application.UseCases.Interfaces;
using Domain.Entities;
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
    public class AgendaUseCase : IAgendaUseCase
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly ILogger<AgendaUseCase> _log;

        public AgendaUseCase(IAgendaRepository agendaRepository, ILogger<AgendaUseCase> log)
        {
            _agendaRepository = agendaRepository;
            _log = log;
        }

        public async Task<Agenda> CreateAgenda(Agenda agenda)
        {
            try
            {
                return await _agendaRepository.CreateAgenda(agenda);
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

        public async Task<Agenda> GetAgendaById(string id)
        {
            try
            {
                var agenda = await _agendaRepository.GetAgendaById(id);
                if (agenda == null) return new Agenda();

                return agenda;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<Agenda>> GetAllAgendas()
        {
            try
            {
                return await _agendaRepository.GetAllAgendas();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAgenda(string id, Agenda agenda)
        {
            try
            {
                var agendaOriginal = await _agendaRepository.GetAgendaById(id);
                if (agendaOriginal is null) throw new Exception("Agenda não encontrado");

                agendaOriginal.Status = agenda.Status;
                agendaOriginal.DataAlteracao = agenda.DataAlteracao;
                agendaOriginal.DataAgendamento = agenda.DataAgendamento;
                agendaOriginal.HorarioAgendamento = agenda.HorarioAgendamento;

                await _agendaRepository.UpdateAgenda(id, agendaOriginal);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAgenda(string id)
        {
            try
            {
                var agendaOriginal = await _agendaRepository.GetAgendaById(id);
                if (agendaOriginal is null) throw new Exception("Agenda não encontrado");

                await _agendaRepository.DeleteAgenda(id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
