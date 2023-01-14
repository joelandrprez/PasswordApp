using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Proyecto
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/proyecto")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Proyecto")]
    public class ProyectoController : ControllerBase
    {
        private readonly ILogger<ProyectoController> _logger;

        public ProyectoController(ILogger<ProyectoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Listar()
        {
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar()
        {
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult> Actualizar()
        {
            return Ok();
        }
    }
}
