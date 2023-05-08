using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cuentas.Backend.Domain.AppAngular.DTO;
using Cuentas.Backend.Shared;
using Cuentas.Backend.Aplication.Generar;

namespace Cuentas.Backend.API.Controllers.Generar
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Generar")]
    public class GenerarController : ControllerBase
    {
        private readonly ILogger<GenerarController> _logger;
        private readonly GenerarApp _proyectoApp;
        public GenerarController(ILogger<GenerarController> logger, GenerarApp proyectoApp)
        {
            this._proyectoApp = proyectoApp;
            _logger = logger;
        }

    }
}
