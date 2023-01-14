using Cuentas.Backend.Aplication.Usuario;
using Cuentas.Backend.Domain.Usuario.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Usuario
{
    [Route("api/v1/usuario")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioApp _usuarioApp;
        public UsuarioController(ILogger<UsuarioController> logger, UsuarioApp usuarioApp)
        {
            _logger = logger;
            _usuarioApp = usuarioApp;
        }

        [HttpPost]
        [Route("registrar")]
        public async Task<ActionResult> Registro([FromBody] InUsuario cuenta)
        {
            StatusSimpleResponse Respuesta = await _usuarioApp.Registrar(cuenta);

            return Ok(Respuesta);
        }

    }
}
