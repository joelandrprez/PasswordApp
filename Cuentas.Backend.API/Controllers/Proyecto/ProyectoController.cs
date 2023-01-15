using Cuentas.Backend.Aplication.Proyecto;
using Cuentas.Backend.Domain.Proyectos.Domain;
using Cuentas.Backend.Domain.Proyectos.DTO;
using Cuentas.Backend.Shared;
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
        private readonly ProyectoApp _proyectoApp;

        public ProyectoController(ILogger<ProyectoController> logger, ProyectoApp proyectoApp)
        {
            _logger = logger;
            _proyectoApp = proyectoApp;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Pagination<ProyectoDominio>> Respuesta = await _proyectoApp.Listar(page, size, search, orderBy, orderDir);
            return StatusCode(Respuesta.Status, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InProyecto cuenta)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _proyectoApp.Registrar(cuenta, int.Parse(CreadoPor));
            return StatusCode(Respuesta.Status, Respuesta);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Actualizar([FromBody] InProyecto cuenta, [FromRoute] int id)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _proyectoApp.Actualizar(cuenta, id, int.Parse(CreadoPor));
            return StatusCode(Respuesta.Status, Respuesta);
        }
    }
}
