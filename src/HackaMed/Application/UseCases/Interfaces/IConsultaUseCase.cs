using Domain.Entities;
using Domain.Helpers.Consulta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Interfaces
{
    public interface IConsultaUseCase
    {
        public Task<Consulta> CreateConsulta(ConsultaInput consulta);
        public Task<IList<Consulta>> GetAllConsultas();
        public Task<Consulta> GetConsultaById(string id);
        public Task UpdateConsulta(string id, Consulta consulta);
        public Task DeleteConsulta(string id);
        public Task UpdateConsultaStatus(string id, string status, string descricao = "");


    }
}
