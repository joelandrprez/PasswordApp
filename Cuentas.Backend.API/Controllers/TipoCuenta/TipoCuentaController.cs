using Cuentas.Backend.API.Controllers.Cuentas;
using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.TipoCuenta
{
    [Route("api/tipoCuenta")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "TipoCuenta")]
    public class TipoCuentaController : ControllerBase
    {
        private readonly ILogger<CuentasController> _logger;


        public TipoCuentaController(ILogger<CuentasController> logger, CuentaApp cuentaApp)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("habilitados")]
        public async Task<ActionResult> ListarHabilitados()
        {
            
            return StatusCode(200, "");
        }
    }
}
