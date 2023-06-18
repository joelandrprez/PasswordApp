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
    public class AccountApp : BaseApp<AccountApp>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountApp(ILogger<BaseApp<AccountApp>> logger, IAccountRepository accountRepository, IConfiguration configuration): base(logger, configuration)
        {
            _accountRepository = accountRepository;
        }

        public async Task<StatusResponse<Pagination<EAccount>>> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Pagination<EAccount>> Respuesta = await this.ComplexProcess(() => _accountRepository.List(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Success)
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;

            Respuesta.Title = Respuesta.StatusCode == StatusCodes.Status200OK ? MaestraConstante.MENSAJE_OPERACION_EXITOSA: MaestraConstante.MENSAJE_ERROR_GENERICO;
            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Save(InAccount account,int idAccountProccess) {
            DateTime FechaRegistro = DateTime.Now;

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false,"");
            InCuentaValidator ValidacionCampos = new InCuentaValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(account);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid) {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Title = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
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

                EAccount cuentaDominio = new();
                cuentaDominio.TipoCuenta_Id = account.TipoCuenta_Id;
                cuentaDominio.Sitio = account.Sitio;
                cuentaDominio.Usuario = account.Usuario;
                cuentaDominio.Password = account.Password;
                cuentaDominio.Usuario_Id = idAccountProccess;
                cuentaDominio.Proyecto_Id = account.Proyecto_Id;
                cuentaDominio.FechaCreacion = FechaProceso;
                cuentaDominio.UsuarioCrea = idAccountProccess;
                cuentaDominio.FechaModificacion = FechaProceso;
                cuentaDominio.UsuarioModificacion = idAccountProccess;

                Respuesta = await this.SimpleProcess(() => _accountRepository.Save(cuentaDominio, conexion, transaction), "");

                if (!Respuesta.Success)
                {
                    Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
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

            Respuesta.Title = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            Respuesta.Success = true;
            return Respuesta;
        
        }

        public async Task<StatusSimpleResponse> Update(InAccount account,int id,int idAccountProccess)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, "");

            InCuentaValidator ValidacionCampos = new InCuentaValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(account);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid)
            {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Title = "Los datos enviados no son válidos";
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

            EAccount cuentaDominio = new();
            cuentaDominio.Id = id;
            cuentaDominio.TipoCuenta_Id = account.TipoCuenta_Id;
            cuentaDominio.Sitio = account.Sitio;
            cuentaDominio.Usuario = account.Usuario;
            cuentaDominio.Password = account.Password;
            cuentaDominio.Usuario_Id = idAccountProccess;
            cuentaDominio.Proyecto_Id = account.Proyecto_Id;
            cuentaDominio.FechaCreacion = FechaProceso;
            cuentaDominio.UsuarioCrea = idAccountProccess;
            cuentaDominio.FechaModificacion = FechaProceso;
            cuentaDominio.UsuarioModificacion = idAccountProccess;


            Respuesta = await this.SimpleProcess(() => _accountRepository.Update(cuentaDominio, conexion, transaction), "");

            if (!Respuesta.Success)
            {
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
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

            Respuesta.Title = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            Respuesta.Success = true;
            return Respuesta;

        }

        public async Task<StatusResponse<OutAccount>> GetPassword(int id)
        {
            StatusResponse<OutAccount> respuesta = new StatusResponse<OutAccount>();
            StatusResponse<EAccount> DatosUsuario = await this.ComplexProcess(() => _accountRepository.GetPassword(id));

            if (!DatosUsuario.Success)
                return new StatusResponse<OutAccount>(false,DatosUsuario.Title,DatosUsuario.Detail,StatusCodes.Status500InternalServerError,DatosUsuario.Errors);

            if (DatosUsuario.Data == null)
                return new StatusResponse<OutAccount>(false, "No se pudo recuperar el dato de la cuenta", "", StatusCodes.Status500InternalServerError, DatosUsuario.Errors);


            OutAccount cuenta = new OutAccount();
            cuenta.Cadena = DatosUsuario.Data.Password;
            respuesta.Data = cuenta;
            respuesta.Title = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            respuesta.Detail = "Se copio la contraseña";

            return respuesta;
        }
        
    }
}
