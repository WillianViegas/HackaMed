using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackaMed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuxiliarController
    {
        private readonly ILogger<AuxiliarController> _logger;
        private readonly IUsuarioUseCase _usuarioUseCase;


        public AuxiliarController(ILogger<AuxiliarController> logger, IUsuarioUseCase usuarioUseCase)
        {
            _logger = logger;
            _usuarioUseCase = usuarioUseCase;
        }

        [HttpGet("/teste")]
        public IResult Teste()
        {
            try
            {
                return TypedResults.Ok("Teste");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [HttpGet("/SeedUsuarios")]
        public async Task<IResult> SeedUsuarios()
        {
            try
            {
                var usuariosCadastrados = await _usuarioUseCase.GetAllUsuarios();

                if(usuariosCadastrados.Any())
                    return TypedResults.Ok("Já existem usuários cadastrados na base");

                var usuarios = GetUsuariosSeed();

                foreach (var usuario in usuarios)
                {
                    await _usuarioUseCase.CreateUsuario(usuario);
                }

                return TypedResults.Ok("Cadastrados usuários na base de dados");
            }
            catch (ValidationException ex)
            {
                var error = "Erro ao criar usuário";

                _logger.LogError(error, ex);
                return TypedResults.BadRequest(error);
            }
            catch (Exception ex)
            {
                var erro = "Erro ao criar usuário";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        private List<Usuario> GetUsuariosSeed()
        {
            var usuarios = new List<Usuario>();

            var medico = new Usuario()
            {
                Nome = "Marcos Almeida",
                CRM = "",
                Especialidade = "Cardiologia",
                Email = "marcos@gmail.com",
                Perfil = EnumPerfil.Medico.ToString(),
                Senha = "Teste@123",
                Endereco = new Endereco()
                {
                    Cep = "09290500",
                    Cidade = "Santos",
                    Bairro = "Serra santa",
                    Estado = "SP",
                    Rua = "Avenida teste",
                    Número = "66",
                    Latitude = "343434",
                    Longitude = "344545"
                },
                DataCadastro = DateTime.Now,
                DataAlteracao = DateTime.Now
               
            };

            var paciente = new Usuario()
            {
                Nome = "Josnei da Silva",
                CPF = "45926744807",
                Email = "josnei@gmail.com",
                Perfil = EnumPerfil.Paciente.ToString(),
                Senha = "Teste@123",
                Endereco = new Endereco()
                {
                    Cep = "08390500",
                    Cidade = "São Paulo",
                    Bairro = "Pq. Judas",
                    Estado = "SP",
                    Rua = "Avenida Dominic",
                    Número = "66",
                    Latitude = "343434",
                    Longitude = "344545"
                },
                DataCadastro = DateTime.Now,
                DataAlteracao = DateTime.Now
            };

            usuarios.Add(medico);
            usuarios.Add(paciente);

            return usuarios;
        }
    }
}
