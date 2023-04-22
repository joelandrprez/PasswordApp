using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Domain.Cuentas.Domain;
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
        [Route("")]
        public async Task<ActionResult> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Pagination<Cuenta>> Respuesta = await _cuentaApp.Listar(page,size,search,orderBy,orderDir);
            return StatusCode(Respuesta.Status, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InCuenta cuenta)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _cuentaApp.Registrar(cuenta,int.Parse(CreadoPor));
            return StatusCode(Respuesta.Status, Respuesta);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Actualizar([FromBody] InCuenta cuenta, [FromRoute] int id)
        {
            string CreadoPor = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault()?.Value;
            StatusSimpleResponse Respuesta = await _cuentaApp.Actualizar(cuenta,id, int.Parse(CreadoPor));
            return StatusCode(Respuesta.Status, Respuesta);
        }
        [HttpGet]
        [Route("getPassword/{id}")]
        public async Task<ActionResult> getPassword(int id)
        {
            StatusResponse<Cuenta> Respuesta = await _cuentaApp.GetPassword(id);
            return StatusCode(Respuesta.Status, Respuesta);
        }
    }
}
