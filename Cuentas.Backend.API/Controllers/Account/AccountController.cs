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
    [Route("api/v1/Account")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AccountApp _cuentaApp;
        private string _usuario = string.Empty;
        public AccountController(ILogger<AccountController> logger, AccountApp cuentaApp)
        {
            _logger = logger;
            _cuentaApp = cuentaApp;
            
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<Pagination<EAccount>> Respuesta = await _cuentaApp.List(page,size,search,orderBy,orderDir);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save([FromBody] InAccount cuenta)
        {
            _usuario = "1";
            //_usuario = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault().Value;

            StatusSimpleResponse Respuesta = await _cuentaApp.Save(cuenta,int.Parse(_usuario));
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update([FromBody] InAccount cuenta, [FromRoute] int id)
        {
            //_usuario = User.Claims.Where(x => x.Type == MaestraConstante.CODIGO_ID_USER_TOKEN).FirstOrDefault().Value;
            _usuario = "joel";
            StatusSimpleResponse Respuesta = await _cuentaApp.Update(cuenta,id, int.Parse(_usuario));
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

        [HttpGet]
        [Route("cadena/{id}")]
        public async Task<ActionResult> getPassword(int id)
        {
            StatusResponse<OutAccount> Respuesta = await _cuentaApp.GetPassword(id);
            return StatusCode(Respuesta.StatusCode, Respuesta);
        }
    }
}
