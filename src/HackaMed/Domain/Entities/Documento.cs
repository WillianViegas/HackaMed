using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Documento
    {
        public string Id { get; set; }
        public string PacienteId { get; set; }
        public string Titulo { get; set; }
        public string Decricao { get; set; }
        public string Url { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
