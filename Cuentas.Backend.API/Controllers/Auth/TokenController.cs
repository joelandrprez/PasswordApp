using Cuentas.Backend.Aplication.Token;
using Cuentas.Backend.Domain.Token.DTO;
using Cuentas.Backend.Domain.Usuario.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Token
{
    [Route("api/v1/token")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Token")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly TokenApp _tokenApp;

        public TokenController(ILogger<TokenController> logger, TokenApp tokenApp)
        {
            _logger = logger;
            _tokenApp = tokenApp;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] InUsuario cuenta)
        {
            StatusResponse<OuUsuarioLogeado> Respuesta = await _tokenApp.Login(cuenta);

            return StatusCode(Respuesta.Status, Respuesta);
        }

    }
}
