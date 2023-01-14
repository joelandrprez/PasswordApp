using System.Security.Cryptography;
using Cuentas.Backend.Domain.Token.DTO;
using Cuentas.Backend.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Cuentas.Backend.Domain.Usuario.DTO;


namespace Cuentas.Backend.Aplication.Token
{
    public class TokenApp 
    {
        private string _key = string.Empty;
        private string _issuer = string.Empty;
        private string _audience = string.Empty;

        public TokenApp(IConfiguration config)
        {
            this._key = config.GetValue<string>("Jwt:Issuer");
            this._issuer = config.GetValue<string>("Jwt:Audience");
            this._audience = config.GetValue<string>("Jwt:Key");
        }

        public async Task<StatusResponse<OuUsuarioLogeado>> Login(InUsuario usuario) {
            StatusResponse<OuUsuarioLogeado> Respuesta = new StatusResponse<OuUsuarioLogeado>();



            Tuple<DateTime, string> Token =  GenerateToken(1);

            OuUsuarioLogeado UsuarioLogeado = new OuUsuarioLogeado();
            UsuarioLogeado.Token = Token.Item2;
            Respuesta.Data = UsuarioLogeado;

            return Respuesta;

        }

        public Tuple<DateTime, string> GenerateToken(int id)
        {
            try
            {

                var key = Encoding.ASCII.GetBytes(_key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("Id", Guid.NewGuid().ToString())
                    }),

                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = this._issuer,
                    Audience = this._audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return new Tuple<DateTime, string>(DateTime.Now, stringToken);
            }
            catch (Exception ex)
            {
                return new Tuple<DateTime, string>(DateTime.Now, ex.Message);
            }



        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(_key);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception ex)
            {

                return null;
            }
        }


    }
}
