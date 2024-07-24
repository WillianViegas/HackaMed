using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Medico
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CRM { get; set; }
        public string Avaliacoes { get; set; }
        public string DistanciaKM { get; set; }
        public string Especialidade { get; set; }
    }
}
