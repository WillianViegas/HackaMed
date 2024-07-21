using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Agenda
    {
        public string Id { get; set; }
        public string MedicoId { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string HorarioAgendamento { get; set; }
        public string Status { get; set; }
        public string DataCadastro { get; set; }
        public string DataAlteracao { get; set; }
    }
}
