using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Menu
{
    [Route("api/Menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            //StatusResponse<Pagination<Cuenta>> Respuesta = await _cuentaApp.Listar(page, size, search, orderBy, orderDir);
            return Ok(new { page, size, search, orderBy, orderDir });
        }
    }
}
