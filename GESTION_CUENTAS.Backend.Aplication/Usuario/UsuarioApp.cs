using Cuentas.Backend.Domain.Usuario.DTO;
using Cuentas.Backend.Shared;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cuentas.Backend.Domain.Usuario.Domain;
using Cuentas.Backend.Infraestruture.Usuario;
using Cuentas.Backend.Aplication.Comun;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Cuentas.Backend.Domain.Usuario.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cuentas.Backend.Aplication.Usuario
{
    public class UsuarioApp:BaseApp<UsuarioApp>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioApp(ILogger<BaseApp<UsuarioApp>> logger, IConfiguration configuracion, IUsuarioRepository usuarioRepository) : base(logger, configuracion)
        {
            _usuarioRepository= usuarioRepository;
        }

        public async Task<StatusSimpleResponse> Registrar(InUsuario usuario)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse();
            DateTime FechaOperacion = DateTime.Now;
            UsuarioPortal UsuarioRegistro = new ();
            UsuarioRegistro.Password = GenerateHashed(usuario.Password);
            UsuarioRegistro.Usuario = usuario.Usuario;
            UsuarioRegistro.Saltd = "autoGenerado";
            UsuarioRegistro.FechaModificacion = FechaOperacion;
            UsuarioRegistro.UsuarioCreacion = 1;
            UsuarioRegistro.UsuarioModificacion = 1;
            UsuarioRegistro.FechaCreacion = FechaOperacion;

            SqlConnection conexion = new();
            SqlTransaction transaction = null;

            try
            {
                conexion = this.ConexionParaTransaccion();
                conexion.Open();
                transaction = conexion.BeginTransaction();
            }
            catch (Exception ex)
            {
                return this.RespuestaDeErrorFormateada(ex);
            }

            try
            {
                StatusResponse<bool> ValidarExistencia = await this.ValidarExistenciaDeNombreDeUsuario(UsuarioRegistro.Usuario, conexion, transaction);

                if (!ValidarExistencia.Satisfactorio)
                    return ValidarExistencia;

                if (ValidarExistencia.Data == MaestraConstante.ESTADO_USUARIO_EXISTE)
                    return this.RespuestaDeErrorFormateada(MaestraConstante.MENSAJE_ESTADO_USUARIO_EXISTE);
                
                Respuesta = await this.ProcesoSimple(() => _usuarioRepository.Registrar(UsuarioRegistro, conexion, transaction), "");

                if (!Respuesta.Satisfactorio)
                    return Respuesta;                  

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return this.RespuestaDeErrorFormateada(ex);

            }
            finally {
                if (transaction != null)
                    transaction.Dispose();

                if (conexion != null)
                {
                    if (conexion.State == System.Data.ConnectionState.Open)
                        conexion.Close();
                    conexion.Dispose();
                }
            }


            Respuesta.Titulo = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            return Respuesta;
        }
        public async Task<StatusResponse<bool>> ValidarExistenciaDeNombreDeUsuario(string usuario, SqlConnection conexion, SqlTransaction transaccion) {

            StatusResponse<UsuarioPortal> Busqueda = new ();
            bool respuesta = MaestraConstante.ESTADO_USUARIO_NO_EXISTE;
            Busqueda = await this.ProcesoComplejo(() => _usuarioRepository.ValidarExistenciaDeNombreDeUsuario(usuario, conexion, transaccion));

            if (Busqueda.Data != null )
                respuesta = MaestraConstante.ESTADO_USUARIO_EXISTE;          

            StatusResponse<bool> Respuesta = new StatusResponse<bool> ();
            Respuesta.Satisfactorio = Busqueda.Satisfactorio;
            Respuesta.Titulo = Busqueda.Titulo;
            Respuesta.Detalle = Busqueda.Detalle;
            Respuesta.Data = respuesta;
            return Respuesta;

        }
        public StatusSimpleResponse RespuestaDeErrorFormateada(Exception exception) {

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, MaestraConstante.MENSAJE_ERROR_GENERICO_CATH);
            Guid id = Guid.NewGuid();
            Respuesta.Id = id;
            this._logger.LogError(exception, "Id: {0}", Respuesta.Id);
            Respuesta.Detalle = exception.ToString();
            return Respuesta;

        }
        public StatusSimpleResponse RespuestaDeErrorFormateada(string exception)
        {

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, MaestraConstante.MENSAJE_ERROR_GENERICO_CATH);
            Guid id = Guid.NewGuid();
            Respuesta.Id = id;
            Respuesta.Titulo = exception;
            return Respuesta;

        }

        public string GenerateHashed(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 100000,
                            numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
