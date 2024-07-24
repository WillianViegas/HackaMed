using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers.Login
{
    public class LoginInput
    {
        public string Email { get; set; }
        public string CPF { get; set; }
        public string CRM { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; }
    }
}
