using Cuentas.Backend.Aplication.Comun;
using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Domain.Cuentas.DTO;
using Cuentas.Backend.Domain.Cuentas.Interfaces;
using Cuentas.Backend.Shared;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Aplication.Cuentas
{
    public class CuentaApp : BaseApp<CuentaApp>
    {
        private readonly ICuentaRepository _usuarioRepository;

        public CuentaApp(ILogger<BaseApp<CuentaApp>> logger, ICuentaRepository cuentaRepository, IConfiguration configuracion): base(logger, configuracion)
        {
            _usuarioRepository = cuentaRepository;
        }

        public async Task<StatusResponse<Paginacion<ECuenta>>> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Paginacion<ECuenta>> Respuesta = await this.ProcesoComplejo(() => _usuarioRepository.Listar(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Satisfactorio)
                Respuesta.Codigo = StatusCodes.Status500InternalServerError;

            Respuesta.Titulo = Respuesta.Codigo == StatusCodes.Status200OK ? MaestraConstante.MENSAJE_OPERACION_EXITOSA: MaestraConstante.MENSAJE_ERROR_GENERICO;
            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Registrar(InCuenta cuenta,int idUsuario) {
            DateTime FechaRegistro = DateTime.Now;

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false,"");
            InCuentaValidator ValidacionCampos = new InCuentaValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(cuenta);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid) {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.Codigo = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }

            SqlConnection conexion = new();
            SqlTransaction transaction = null;

            try
            {
                conexion = this.ConectionToTransaction();
                conexion.Open();
                transaction = conexion.BeginTransaction();
            }
            catch (Exception ex)
            {
                return new(false, "Error al crear la conexion", ex.Message, StatusCodes.Status500InternalServerError);
            }
            try {

                ECuenta cuentaDominio = new();
                cuentaDominio.TipoCuenta_Id = cuenta.TipoCuenta_Id;
                cuentaDominio.Sitio = cuenta.Sitio;
                cuentaDominio.Usuario = cuenta.Usuario;
                cuentaDominio.Password = cuenta.Password;
                cuentaDominio.Usuario_Id = idUsuario;
                cuentaDominio.Proyecto_Id = cuenta.Proyecto_Id;
                cuentaDominio.FechaCreacion = FechaProceso;
                cuentaDominio.UsuarioCrea = idUsuario;
                cuentaDominio.FechaModificacion = FechaProceso;
                cuentaDominio.UsuarioModificacion = idUsuario;

                Respuesta = await this.ProcesoSimple(() => _usuarioRepository.Registrar(cuentaDominio, conexion, transaction), "");

                if (!Respuesta.Satisfactorio)
                {
                    Respuesta.Codigo = StatusCodes.Status500InternalServerError;
                    return Respuesta;
                }

                transaction.Commit();
            }
            catch (Exception ex) {
                transaction.Rollback();
                return new(false, "Ocurrio un error al registrar la cuenta", ex.Message, StatusCodes.Status500InternalServerError);

            }
            finally
            {
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
            Respuesta.Satisfactorio = true;
            return Respuesta;
        
        }

        public async Task<StatusSimpleResponse> Actualizar(InCuenta cuenta,int id,int idUsuario)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, "");

            InCuentaValidator ValidacionCampos = new InCuentaValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(cuenta);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid)
            {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(ResultadoValidacion.Errors);
                return Respuesta;
            }

            SqlConnection conexion = new();
            SqlTransaction transaction = null;

            try
            {
                conexion = this.ConectionToTransaction();
                conexion.Open();
                transaction = conexion.BeginTransaction();
            }
            catch (Exception ex)
            {
                return new(false, "Error al crear la conexion", ex.Message, StatusCodes.Status500InternalServerError);
            }
            try
            {

            ECuenta cuentaDominio = new();
            cuentaDominio.Id = id;
            cuentaDominio.TipoCuenta_Id = cuenta.TipoCuenta_Id;
            cuentaDominio.Sitio = cuenta.Sitio;
            cuentaDominio.Usuario = cuenta.Usuario;
            cuentaDominio.Password = cuenta.Password;
            cuentaDominio.Usuario_Id = idUsuario;
            cuentaDominio.Proyecto_Id = cuenta.Proyecto_Id;
            cuentaDominio.FechaCreacion = FechaProceso;
            cuentaDominio.UsuarioCrea = idUsuario;
            cuentaDominio.FechaModificacion = FechaProceso;
            cuentaDominio.UsuarioModificacion = idUsuario;


            Respuesta = await this.ProcesoSimple(() => _usuarioRepository.Actualizar(cuentaDominio, conexion, transaction), "");

            if (!Respuesta.Satisfactorio)
            {
                Respuesta.Codigo = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }

            transaction.Commit();
            }
            catch (Exception ex) {
                transaction.Rollback();
                return new (false, "Ocurrio un error al actualizar la cuenta", ex.Message, StatusCodes.Status500InternalServerError);

            }
            finally
            {
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
            Respuesta.Satisfactorio = true;
            return Respuesta;

        }

        public async Task<StatusResponse<OutCuenta>> ObtenerPassword(int id)
        {
            StatusResponse<OutCuenta> respuesta = new StatusResponse<OutCuenta>();
            StatusResponse<ECuenta> DatosUsuario = await this.ProcesoComplejo(() => _usuarioRepository.ObtenerPassword(id));

            if (!DatosUsuario.Satisfactorio)
                return new StatusResponse<OutCuenta>(false,DatosUsuario.Titulo,DatosUsuario.Detalle,StatusCodes.Status500InternalServerError,DatosUsuario.Errors);

            if (DatosUsuario.Data == null)
                return new StatusResponse<OutCuenta>(false, "No se pudo recuperar el dato de la cuenta", "", StatusCodes.Status500InternalServerError, DatosUsuario.Errors);


            OutCuenta cuenta = new OutCuenta();
            cuenta.Cadena = DatosUsuario.Data.Password;
            respuesta.Data = cuenta;
            respuesta.Titulo = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            respuesta.Detalle = "Se copio la contraseña";

            return respuesta;
        }
        
    }
}
