using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IConsultaRepository
    {
        public Task<Consulta> CreateConsulta(Consulta consulta);
        public Task<IList<Consulta>> GetAllConsultas();
        public Task<Consulta> GetConsultaById(string id);
        public Task UpdateConsulta(string id, Consulta usuario);
        public Task DeleteConsulta(string id);
    }
}
