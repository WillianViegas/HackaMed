using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers.Consulta
{
    public class ConsultaInput
    {
        public string PacienteId { get; set; }
        public string MedicoId { get; set; }
        public string AgendamentoId { get; set; }
    }
}
