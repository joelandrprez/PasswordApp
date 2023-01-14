using Cuentas.Backend.Aplication.Usuario;
using Cuentas.Backend.Domain.Usuario.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Usuario
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InUsuario cuenta)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _usuarioApp.Registrar(cuenta,CreadoPor);
            return StatusCode(Respuesta.Status, Respuesta); 
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<ActionResult> Actualizar([FromBody] InUsuario cuenta,[FromRoute]int Id)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _usuarioApp.Actualizar(cuenta, Id, CreadoPor);
            return StatusCode(Respuesta.Status, Respuesta);
        }

    }
}
