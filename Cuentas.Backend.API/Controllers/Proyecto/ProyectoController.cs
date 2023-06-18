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
    [Route("api/v1/Proyecto")]
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
        public async Task<ActionResult> Listar(int? pagina, int? tamanio, string? termino, string? ordenarPor, string? ordenarDir)
        {
            StatusResponse<Paginacion<Domain.Proyectos.Domain.EProyecto>> Respuesta = await _proyectoApp.List(pagina, tamanio, termino, ordenarPor, ordenarDir);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InProyecto cuenta)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _proyectoApp.Save(cuenta, int.Parse(CreadoPor));
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Actualizar([FromBody] InProyecto cuenta, [FromRoute] int id)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _proyectoApp.Update(cuenta, id, int.Parse(CreadoPor));
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }
    }
}
