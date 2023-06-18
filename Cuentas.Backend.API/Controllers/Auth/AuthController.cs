using Cuentas.Backend.Aplication.Token;
using Cuentas.Backend.Domain.Token.DTO;
using Cuentas.Backend.Domain.Usuario.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cuentas.Backend.API.Controllers.Token
{
    [Route("api/v1/Auth")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthApp _authApp;

        public AuthController(ILogger<AuthController> logger, AuthApp authApp)
        {
            _logger = logger;
            _authApp = authApp;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] InUsuario usuario)
        {
            StatusResponse<OutUsuario> Respuesta = await _authApp.Login(usuario);

            return StatusCode(Respuesta.StatusCode, Respuesta);
        }

    }
}
