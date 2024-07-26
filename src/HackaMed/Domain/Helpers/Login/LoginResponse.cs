using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers.Login
{
    public class LoginResponse
    {
        public string UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string CRM { get; set; }
        public string Perfil { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
    }
}
