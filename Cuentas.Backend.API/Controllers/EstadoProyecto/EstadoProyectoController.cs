using Cuentas.Backend.Aplication.EstadoProyecto;
using Cuentas.Backend.Domain.EstadoProyecto.Domain;
using Cuentas.Backend.Domain.EstadoProyecto.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.EstadoProyecto
{ 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/EstadoProyecto")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "EstadoProyecto")]
    public class EstadoProyectoController : ControllerBase
    {
        private readonly ILogger<EstadoProyectoController> _logger;
        private readonly EstadoProyectoApp _estadoProyectoApp;

        public EstadoProyectoController(ILogger<EstadoProyectoController> logger, EstadoProyectoApp estadoProyectoApp)
        {
            _logger = logger;
            _estadoProyectoApp = estadoProyectoApp; 
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Paginacion<EEstadoProyecto>> Respuesta = await _estadoProyectoApp.List(page, size, search, orderBy, orderDir);
            return StatusCode(Respuesta.Codigo, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InEstadoProyecto estadoProyecto)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _estadoProyectoApp.Save(estadoProyecto, CreadoPor);
            return StatusCode(Respuesta.Codigo, Respuesta);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Actualizar([FromBody] InEstadoProyecto estadoProyecto,[FromRoute] int id)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _estadoProyectoApp.Update(estadoProyecto, id, CreadoPor);
            return StatusCode(Respuesta.Codigo, Respuesta);
        }
    }
}
