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
        private readonly IUsuarioRepository _userRepository;
        private const int Pbkdf2Iterations = 1000;

        public AuthApp(ILogger<BaseApp<AuthApp>> logger, IConfiguration configuracion, IUsuarioRepository userRepository) : base(logger, configuracion)
        {
            this._key = configuracion.GetValue<string>("Jwt:Key"); 
            this._issuer = configuracion.GetValue<string>("Jwt:Issuer");
            this._audience = configuracion.GetValue<string>("Jwt:Audience"); 
            this._userRepository = userRepository;
        }

        public async Task<StatusResponse<OutUsuario>> Login(InUsuario user) {


            StatusResponse<OutUsuario> Respuesta = new StatusResponse<OutUsuario>();

            InUsuarioValidator validator = new InUsuarioValidator();
            ValidationResult resultadoValidacion = validator.Validate(user);

            if (!resultadoValidacion.IsValid)
                return new StatusResponse<OutUsuario>(false, "Datos no validos", "", StatusCodes.Status400BadRequest, this.GetErrors(resultadoValidacion.Errors));

            StatusResponse<EUsuario> Validacion = await this.ProcesoComplejo(() => _userRepository.ValidarExistenciaDeNombreDeUsuarioSinTransaccion(user.NombreUsuario));

            if (!Validacion.Satisfactorio )
                return new StatusResponse<OutUsuario>(false,Validacion.Titulo,Validacion.Detalle, StatusCodes.Status500InternalServerError);

            if ( Validacion.Data == null)
                return new StatusResponse<OutUsuario>(false, "Usuario no registrado", Validacion.Detalle, StatusCodes.Status400BadRequest);

            var validacionPassword =  this.PasswordValidation(user.Password, Validacion.Data.Password);

            if (!validacionPassword.Data) 
                return new StatusResponse<OutUsuario>(false, validacionPassword.Titulo, validacionPassword.Detalle,StatusCodes.Status406NotAcceptable);


            StatusResponse<string> Token =  GenerateToken(Validacion.Data.Id);

            OutUsuario UsuarioLogeado = new OutUsuario();
            UsuarioLogeado.Token = Token.Data;
            Respuesta.Data = UsuarioLogeado;
            Respuesta.Titulo = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            return Respuesta;

        }

        public StatusResponse<bool> PasswordValidation(string passwordSend,string passwordRegister) {

            StatusResponse<bool> Respuesta = new();
            bool ContraseniaEnviadaConHash = false;
            ContraseniaEnviadaConHash =   this.VerifyHashedPasswordV3(passwordRegister, passwordSend);
            Respuesta.Titulo = ContraseniaEnviadaConHash == true ? "" : "Error con las credenciales";
            Respuesta.Satisfactorio = true;
            Respuesta.Data = ContraseniaEnviadaConHash;

            return Respuesta;
        }

        public StatusResponse<string> GenerateToken(int id)
        {
            StatusResponse<string> Respuesta = new();
            try
            {

                var key = Encoding.ASCII.GetBytes(_key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("IdUser",id.ToString())
                    }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature),
                    Issuer = _issuer,
                    Audience = _audience,
                    Expires = DateTime.UtcNow.AddHours(5)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                Respuesta.Data = tokenHandler.WriteToken(token);
                return Respuesta;
            }
            catch (Exception ex)
            {
                Respuesta.Satisfactorio = false;
                Respuesta.Titulo = ex.Message;
                Respuesta.Codigo = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }



        }

        public ClaimsPrincipal GetMain(string token)
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
