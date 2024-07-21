using Microsoft.AspNetCore.Mvc;

namespace HackaMed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : ControllerBase
    {
        private readonly ILogger<ConsultaController> _logger;

        public ConsultaController(ILogger<ConsultaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/teste-consulta")]
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
    }
}
