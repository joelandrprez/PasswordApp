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
    [Route("api/v1/ProjectStatus")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ProjectStatus")]
    public class ProjectStatusController : ControllerBase
    {
        private readonly ILogger<ProjectStatusController> _logger;
        private readonly ProjectStatusApp _estadoProyectoApp;

        public ProjectStatusController(ILogger<ProjectStatusController> logger, ProjectStatusApp estadoProyectoApp)
        {
            _logger = logger;
            _estadoProyectoApp = estadoProyectoApp; 
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Pagination<EProjectStatus>> Respuesta = await _estadoProyectoApp.List(page, size, search, orderBy, orderDir);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save([FromBody] InProjectStatus estadoProyecto)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _estadoProyectoApp.Save(estadoProyecto, CreadoPor);
            return StatusCode(Respuesta.StatusCode, Respuesta);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update([FromBody] InProjectStatus estadoProyecto,[FromRoute] int id)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _estadoProyectoApp.Update(estadoProyecto, id, CreadoPor);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }
    }
}
