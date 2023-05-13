using Microsoft.AspNetCore.Mvc;
using Cuentas.Backend.Domain.AppAngular.DTO;
using Cuentas.Backend.Aplication.Generar;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Cuentas.Backend.API.Controllers.Generar
{
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/generar")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Generar")]
    public class GenerarController : ControllerBase
    {
        private readonly ILogger<GenerarController> _logger;
        private readonly GenerarApp _proyectoApp;
        public GenerarController(ILogger<GenerarController> logger, GenerarApp proyectoApp)
        {
            _proyectoApp = proyectoApp;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Generar(InGenerarApp proyecto)
        {
            StatusSimpleResponse Respuesta = await _proyectoApp.GenerarEstructura(proyecto);
            return StatusCode(Respuesta.Status, Respuesta);

        }


    }
}
