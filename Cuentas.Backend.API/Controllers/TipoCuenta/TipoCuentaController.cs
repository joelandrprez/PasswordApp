using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.TipoCuenta
{
    [Route("api/TipoCuenta")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "TipoCuenta")]
    public class TipoCuentaController : ControllerBase
    {
        private readonly ILogger<TipoCuentaController> _logger;


        public TipoCuentaController(ILogger<TipoCuentaController> logger, CuentaApp cuentaApp)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("ListarHabilitados")]
        public async Task<ActionResult> ListarHabilitados()
        {
            
            return StatusCode(200, "");
        }
    }
}
