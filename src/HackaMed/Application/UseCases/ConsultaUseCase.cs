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
                consultaOriginal.LinkConsulta = consulta.LinkConsulta;
                consultaOriginal.DataConsulta = consulta.DataConsulta;
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
    }
}
