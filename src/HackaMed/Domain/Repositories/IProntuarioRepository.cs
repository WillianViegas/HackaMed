using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProntuarioRepository
    {
        public Task<Prontuario> CreateProntuario(Prontuario prontuario);
        public Task<Prontuario> GetProntuarioByPacienteId(string id);
        public Task UpdateProntuario(string idProntuario, Prontuario prontuario);
    }
}
