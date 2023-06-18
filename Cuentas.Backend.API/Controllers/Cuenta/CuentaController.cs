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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/Cuenta")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Cuenta")]
    public class CuentaController : ControllerBase
    {
        private readonly ILogger<CuentaController> _logger;
        private readonly CuentaApp _cuentaApp;
        private string _usuario = string.Empty;
        public CuentaController(ILogger<CuentaController> logger, CuentaApp cuentaApp)
        {
            _logger = logger;
            _cuentaApp = cuentaApp;
            
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Paginacion<ECuenta>> Respuesta = await _cuentaApp.Listar(page,size,search,orderBy,orderDir);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Registrar([FromBody] InCuenta cuenta)
        {
            _usuario = "1";
            //_usuario = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault().Value;

            StatusSimpleResponse Respuesta = await _cuentaApp.Registrar(cuenta,int.Parse(_usuario));
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Actualizar([FromBody] InCuenta cuenta, [FromRoute] int id)
        {
            //_usuario = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault().Value;
            _usuario = "joel";
            StatusSimpleResponse Respuesta = await _cuentaApp.Actualizar(cuenta,id, int.Parse(_usuario));
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpGet]
        [Route("cadena/{id}")]
        public async Task<ActionResult> ObtenerPassword(int id)
        {
            StatusResponse<OutCuenta> Respuesta = await _cuentaApp.ObtenerPassword(id);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }
    }
}
