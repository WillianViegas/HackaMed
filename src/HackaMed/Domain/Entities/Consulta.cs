using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Consulta
    {
        public string Id { get; set; }
        public string PacienteId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string LinkConsulta { get; set; }
        public string Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
