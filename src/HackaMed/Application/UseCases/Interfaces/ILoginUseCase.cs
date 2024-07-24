using Domain.Entities;
using Domain.Helpers.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Interfaces
{
    public interface ILoginUseCase
    {
        public Task<LoginResponse> Login(LoginInput loginInput);
    }
}
