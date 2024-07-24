using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAgendaRepository
    {
        public Task<Agenda> CreateAgenda(Agenda agenda);
        public Task<IList<Agenda>> GetAllAgendas();
        public Task<Agenda> GetAgendaById(string id);
        public Task UpdateAgenda(string id, Agenda agenda);
        public Task DeleteAgenda(string id);
        public Task<IList<Agenda>> GetAgendaByMedicoId(string id);
    }
}
