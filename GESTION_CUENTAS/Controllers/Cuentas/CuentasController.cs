using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Domain.Cuentas.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Cuentas
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/cuentas")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly ILogger<CuentasController> _logger;
        private readonly CuentaApp _cuentaApp;

        public CuentasController(ILogger<CuentasController> logger, CuentaApp cuentaApp)
        {
            _logger = logger;
            _cuentaApp = cuentaApp;
        }
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult> Search(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Pagination<OutCuenta>> status = new StatusResponse<Pagination<OutCuenta>>();
            if (!status.Satisfactorio)
                return StatusCode(StatusCodes.Status500InternalServerError, status);

            return Ok(status);
        }
        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InCuenta cuenta)
        {
            StatusSimpleResponse Respuesta = await _cuentaApp.Registrar(cuenta);

            return Ok(Respuesta);
        }
        [HttpPut]
        [Route("")]
        public async Task<ActionResult> Actualizar([FromRoute] InCuenta cuenta)
        {
            return Ok();
        }
    }
}
