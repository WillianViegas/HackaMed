using Domain.Entities;
using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Interfaces
{
    public interface IPacienteUseCase
    {
        public Task<IList<Medico>> GetAllMedicos(MedicoFilter medicoFilter);
        public Task<Prontuario> CadastrarProntuario(Prontuario prontuario);
        public Task<Prontuario> AdicionarDocumento(Documento documento);
        public Task<Prontuario> RemoverDocumento(Documento documento);
    }
}
