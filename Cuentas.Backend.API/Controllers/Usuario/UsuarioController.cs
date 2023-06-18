using Cuentas.Backend.Aplication.Usuario;
using Cuentas.Backend.Domain.Usuario.Domain;
using Cuentas.Backend.Domain.Usuario.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Usuario
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/Usuario")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioApp _usuarioApp;
        private string _usuario = string.Empty;
        public UsuarioController(ILogger<UsuarioController> logger, UsuarioApp usuarioApp)
        {
            _logger = logger;
            _usuarioApp = usuarioApp;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Paginacion<EUsuario>> Respuesta = await _usuarioApp.Listar(page, size, search, orderBy, orderDir);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InUsuario cuenta)
        {
            _usuario = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _usuarioApp.Registrar(cuenta, _usuario);
            return StatusCode(Respuesta.StatusCode, Respuesta); 
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<ActionResult> Actualizar([FromBody] InUsuario cuenta,[FromRoute]int Id)
        {
            _usuario = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _usuarioApp.Actualizar(cuenta, Id, _usuario);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

    }
}
