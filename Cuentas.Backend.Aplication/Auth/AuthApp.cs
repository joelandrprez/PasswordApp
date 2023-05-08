using System.Security.Cryptography;
using Cuentas.Backend.Domain.Token.DTO;
using Cuentas.Backend.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cuentas.Backend.Domain.Usuario.DTO;
using Cuentas.Backend.Domain.Usuario.Interfaces;
using Cuentas.Backend.Domain.Usuario.Domain;
using Cuentas.Backend.Aplication.Comun;
using Microsoft.Extensions.Logging;
using Cuentas.Backend.Aplication.Usuario;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using FluentValidation.Results;


namespace Cuentas.Backend.Aplication.Token
{
    public class AuthApp : BaseApp<AuthApp>
    {
        private string _key = string.Empty;
        private string _issuer = string.Empty;
        private string _audience = string.Empty;
        private readonly IUsuarioRepository _usuarioRepository;
        private const int Pbkdf2Iterations = 1000;

        public AuthApp(ILogger<BaseApp<AuthApp>> logger, IConfiguration configuracion, IUsuarioRepository usuarioRepository) : base(logger, configuracion)
        {
            this._key = configuracion.GetValue<string>("Jwt:Key"); 
            this._issuer = configuracion.GetValue<string>("Jwt:Issuer");
            this._audience = configuracion.GetValue<string>("Jwt:Audience"); 
            this._usuarioRepository = usuarioRepository;
        }

        public async Task<StatusResponse<OutUsuarioLogeado>> Login(InUsuario usuario) {


            StatusResponse<OutUsuarioLogeado> Respuesta = new StatusResponse<OutUsuarioLogeado>();

            InUsuarioValidator validator = new InUsuarioValidator();
            ValidationResult resultadoValidacion = validator.Validate(usuario);

            if (!resultadoValidacion.IsValid)
                return new StatusResponse<OutUsuarioLogeado>(false, "Datos no validos", "", StatusCodes.Status400BadRequest, this.GetErrors(resultadoValidacion.Errors));

            StatusResponse<UsuarioPortal> Validacion = await this.ProcesoComplejo(() => _usuarioRepository.ValidarExistenciaDeNombreDeUsuarioSinTransaccion(usuario.NombreUsuario));

            if (!Validacion.Satisfactorio )
                return new StatusResponse<OutUsuarioLogeado>(false,Validacion.Titulo,Validacion.Detalle, StatusCodes.Status500InternalServerError);

            if ( Validacion.Data == null)
                return new StatusResponse<OutUsuarioLogeado>(false, "Usuario no registrado", Validacion.Detalle, StatusCodes.Status400BadRequest);

            var validacionPassword =  this.ValidacionContrasenia(usuario.Password, Validacion.Data.Password);

            if (!validacionPassword.Data) 
                return new StatusResponse<OutUsuarioLogeado>(false, validacionPassword.Titulo, validacionPassword.Detalle,StatusCodes.Status406NotAcceptable);
            

            Tuple<DateTime, string> Token =  GenerateToken(Validacion.Data.Id);

            OutUsuarioLogeado UsuarioLogeado = new OutUsuarioLogeado();
            UsuarioLogeado.Token = Token.Item2;
            Respuesta.Data = UsuarioLogeado;
            Respuesta.Titulo = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            return Respuesta;

        }

        public StatusResponse<bool> ValidacionContrasenia(string ContraseniaEnviada,string contraseniaGuardada) {

            StatusResponse<bool> Respuesta = new();
            bool ContraseniaEnviadaConHash = false;
            ContraseniaEnviadaConHash =   this.VerifyHashedPasswordV3(contraseniaGuardada, ContraseniaEnviada);
            Respuesta.Titulo = ContraseniaEnviadaConHash == true ? "" : "Error con las credenciales";
            Respuesta.Satisfactorio = true;
            Respuesta.Data = ContraseniaEnviadaConHash;

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
                    new Claim("IdUser",id.ToString())
                    }),

                    Expires = DateTime.UtcNow.AddHours(5),
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
                //TODO registrar en el log cuando se registre un error
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
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception ex)
            {
                //TODO registrar en el log cuando se registre un error
                return null;
            }
        }

        public string HashPasswordV3(string password)
        {
            return Convert.ToBase64String(HashPasswordV3(password, RandomNumberGenerator.Create()
                , prf: KeyDerivationPrf.HMACSHA512, iterCount: Pbkdf2Iterations, saltSize: 128 / 8
                , numBytesRequested: 256 / 8));
        }

        public bool VerifyHashedPasswordV3(string hashedPasswordStr, string password)
        {
            byte[] hashedPassword = Convert.FromBase64String(hashedPasswordStr);
            var iterCount = default(int);
            var prf = default(KeyDerivationPrf);

            try
            {
                prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                if (saltLength < 128 / 8)
                    return false;
                
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);
                int subkeyLength = hashedPassword.Length - 13 - salt.Length;

                if (subkeyLength < 128 / 8)
                    return false;
                
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                
                return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);

            }
            catch(Exception ex)
            {
                //TODO registrar en el log cuando se registre un error
                return false;
            }
        }

        private static byte[] HashPasswordV3(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested)
        {
            byte[] salt = new byte[saltSize];
            rng.GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);
            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; 
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return outputBytes;
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }
        
    }
}
