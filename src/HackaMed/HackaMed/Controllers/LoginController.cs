using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Helpers.Consulta;
using Domain.Helpers.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HackaMed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginUseCase _loginUseCase;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _durationMinutes;

        public LoginController(ILogger<LoginController> logger, ILoginUseCase loginUseCase, IConfiguration configuration)
        {
            _logger = logger;
            _loginUseCase = loginUseCase;
            _jwtKey = configuration.GetSection("Jwt").GetSection("Key").Value;
            _jwtIssuer = configuration.GetSection("Jwt").GetSection("Issuer").Value;
            _jwtAudience = configuration.GetSection("Jwt").GetSection("Audience").Value;
            _durationMinutes = configuration.GetSection("Jwt").GetSection("DurationInMinutes").Value;
        }

        [HttpPost]
        public async Task<IResult> Login(LoginInput loginInput)
        {
            try
            {
                if (loginInput is null)
                    return TypedResults.BadRequest("Dados de login inválidos");

                var usuario = await _loginUseCase.Login(loginInput);

                if (usuario is null)
                    throw new ValidationException("Dados inválidos");

                var token = GenerateJwtToken();

                usuario.Token = token;
                return TypedResults.Ok(usuario);
            }
            catch (ValidationException ex)
            {
                var error = "Erro ao realizar login";

                _logger.LogError(error, ex);
                return TypedResults.BadRequest(error);
            }
            catch (Exception ex)
            {
                var erro = "Erro ao realizar login";
                _logger.LogError(erro, ex);
                return TypedResults.Problem(erro);
            }
        }

        private string GenerateJwtToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "usuario"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_durationMinutes)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
