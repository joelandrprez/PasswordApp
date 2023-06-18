using Cuentas.Backend.API.Controllers.Cuentas;
using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.TipoCuenta
{
    [Route("api/AccountType")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "AccountType")]
    public class AccountTypeController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;


        public AccountTypeController(ILogger<AccountController> logger, AccountApp cuentaApp)
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
