using Cuentas.Backend.API.Controllers.Proyecto;
using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Aplication.Menu;
using Cuentas.Backend.Aplication.Proyecto;
using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Domain.Menu.Domain;
using Cuentas.Backend.Domain.Menu.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Menu
{
    [Route("api/Menu")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Menu")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly MenuApp _menuApp;

        public MenuController(ILogger<MenuController> logger, MenuApp menuApp)
        {
            _logger = logger;
            _menuApp = menuApp;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Listar()
        {
            StatusResponse<List<OutMenu>> Respuesta = await this._menuApp.List(1);
            return StatusCode(Respuesta.StatusCode,Respuesta);
        }
    }
}
