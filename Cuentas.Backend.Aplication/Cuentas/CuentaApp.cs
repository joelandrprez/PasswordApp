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
        private readonly ICuentaRepository _cuentaRepository;

        public CuentaApp(ILogger<BaseApp<CuentaApp>> logger, ICuentaRepository cuentaRepository, IConfiguration configuration): base(logger, configuration)
        {
            _cuentaRepository = cuentaRepository;
        }

        public async Task<StatusResponse<Pagination<Cuenta>>> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Pagination<Cuenta>> Respuesta = await this.ProcesoComplejo(() => _cuentaRepository.Listar(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Satisfactorio)
                Respuesta.Status = StatusCodes.Status500InternalServerError;

            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Registrar(InCuenta cuenta,int idUsuarioProceso) {
            DateTime FechaRegistro = DateTime.Now;

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false,"");
            InCuentaValidator ValidacionCampos = new InCuentaValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(cuenta);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid) {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errores = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.Status = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }

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
                return new(false, "Error al crear la conexion", ex.Message, StatusCodes.Status500InternalServerError);
            }
            try {

                Cuenta cuentaDominio = new();
                cuentaDominio.TipoCuenta_Id = cuenta.TipoCuenta_Id;
                cuentaDominio.Sitio = cuenta.Sitio;
                cuentaDominio.Usuario = cuenta.Usuario;
                cuentaDominio.Password = cuenta.Password;
                cuentaDominio.Usuario_Id = idUsuarioProceso;
                cuentaDominio.Proyecto_Id = cuenta.Proyecto_Id;
                cuentaDominio.FechaCreacion = FechaProceso;
                cuentaDominio.UsuarioCrea = idUsuarioProceso;
                cuentaDominio.FechaModificacion = FechaProceso;
                cuentaDominio.UsuarioModificacion = idUsuarioProceso;

                Respuesta = await this.ProcesoSimple(() => _cuentaRepository.Registrar(cuentaDominio, conexion, transaction), "");

                if (!Respuesta.Satisfactorio)
                {
                    Respuesta.Status = StatusCodes.Status500InternalServerError;
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

        public async Task<StatusSimpleResponse> Actualizar(InCuenta cuenta,int id,int idUsuarioProceso)
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
                Respuesta.Errores = this.GetErrors(ResultadoValidacion.Errors);
                return Respuesta;
            }

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
                return new(false, "Error al crear la conexion", ex.Message, StatusCodes.Status500InternalServerError);
            }
            try
            {

            Cuenta cuentaDominio = new();
            cuentaDominio.Id = id;
            cuentaDominio.TipoCuenta_Id = cuenta.TipoCuenta_Id;
            cuentaDominio.Sitio = cuenta.Sitio;
            cuentaDominio.Usuario = cuenta.Usuario;
            cuentaDominio.Password = cuenta.Password;
            cuentaDominio.Usuario_Id = idUsuarioProceso;
            cuentaDominio.Proyecto_Id = cuenta.Proyecto_Id;
            cuentaDominio.FechaCreacion = FechaProceso;
            cuentaDominio.UsuarioCrea = idUsuarioProceso;
            cuentaDominio.FechaModificacion = FechaProceso;
            cuentaDominio.UsuarioModificacion = idUsuarioProceso;


            Respuesta = await this.ProcesoSimple(() => _cuentaRepository.Actualizar(cuentaDominio, conexion, transaction), "");

            if (!Respuesta.Satisfactorio)
            {
                Respuesta.Status = StatusCodes.Status500InternalServerError;
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
    }
}
