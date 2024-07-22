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
    public class ConsultaUseCase : IConsultaUseCase
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly ILogger<ConsultaUseCase> _log;

        public ConsultaUseCase(IConsultaRepository consultaRepository, ILogger<ConsultaUseCase> log)
        {
            _consultaRepository = consultaRepository;
            _log = log;
        }

        public async Task<Consulta> CreateConsulta(Consulta consulta)
        {
            try
            {
                return await _consultaRepository.CreateConsulta(consulta);
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
