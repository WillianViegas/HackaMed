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
    }
}
