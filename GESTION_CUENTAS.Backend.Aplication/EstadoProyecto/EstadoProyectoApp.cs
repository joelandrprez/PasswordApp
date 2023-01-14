using Cuentas.Backend.Aplication.Comun;
using Cuentas.Backend.Domain.EstadoProyecto.Domain;
using Cuentas.Backend.Domain.EstadoProyecto.DTO;
using Cuentas.Backend.Domain.EstadoProyecto.Interfaces;
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

namespace Cuentas.Backend.Aplication.EstadoProyecto
{
    public class EstadoProyectoApp : BaseApp<EstadoProyectoApp>
    {
        private readonly IEstadoProyectoRepository _estadoProyectoRepository;

        public EstadoProyectoApp(ILogger<BaseApp<EstadoProyectoApp>> logger, IConfiguration configuracion, IEstadoProyectoRepository estadoProyectoRepository) : base(logger, configuracion)
        {
            _estadoProyectoRepository = estadoProyectoRepository;
        }
        public async Task<StatusResponse<Pagination<EstadoProject>>> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Pagination<EstadoProject>> Respuesta = await this.ProcesoComplejo(() => _estadoProyectoRepository.Listar(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Satisfactorio)
                Respuesta.Status = StatusCodes.Status500InternalServerError;

            return Respuesta;
        }
        public async Task<StatusSimpleResponse> Registrar(InEstadoProyecto estadoProyecto, string creadoPor)
        {

            StatusSimpleResponse Respuesta = new StatusSimpleResponse();
            InEstadoProyectoValidator validator = new InEstadoProyectoValidator();
            ValidationResult result = validator.Validate(estadoProyecto);
            if (!result.IsValid)
            {
                Respuesta.Satisfactorio = false;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errores = this.GetErrors(result.Errors);
                Respuesta.Status = StatusCodes.Status400BadRequest;
                return Respuesta;
            }

            DateTime FechaOperacion = DateTime.Now;
            EstadoProject EstadoProyecto = new();
            EstadoProyecto.Estado = estadoProyecto.Estado;
            EstadoProyecto.Descripcion = estadoProyecto.Descripcion;
            EstadoProyecto.FechaModificacion = FechaOperacion;
            EstadoProyecto.UsuarioCrea = int.Parse(creadoPor);
            EstadoProyecto.FechaCreacion = FechaOperacion;
            EstadoProyecto.UsuarioModifica = int.Parse(creadoPor);

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

                Respuesta = await this.ProcesoSimple(() => _estadoProyectoRepository.Registrar(EstadoProyecto, conexion, transaction), "");

                if (!Respuesta.Satisfactorio)
                {
                    Respuesta.Status = StatusCodes.Status500InternalServerError;
                    return Respuesta;
                }

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new(false, "El usuario ya esta registrado", ex.Message, StatusCodes.Status400BadRequest);

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
            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Actualizar(InEstadoProyecto estadoProyecto,int id, string creadoPor)
        {

            StatusSimpleResponse Respuesta = new StatusSimpleResponse();
            InEstadoProyectoValidator validator = new InEstadoProyectoValidator();
            ValidationResult result = validator.Validate(estadoProyecto);
            if (!result.IsValid)
            {
                Respuesta.Satisfactorio = false;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errores = this.GetErrors(result.Errors);
                Respuesta.Status = StatusCodes.Status400BadRequest;
                return Respuesta;
            }

            DateTime FechaOperacion = DateTime.Now;
            EstadoProject EstadoProyecto = new();
            EstadoProyecto.Id = id;
            EstadoProyecto.Estado = estadoProyecto.Estado;
            EstadoProyecto.Descripcion = estadoProyecto.Descripcion;
            EstadoProyecto.FechaModificacion = FechaOperacion;
            EstadoProyecto.UsuarioCrea = int.Parse(creadoPor);
            EstadoProyecto.FechaCreacion = FechaOperacion;
            EstadoProyecto.UsuarioModifica = int.Parse(creadoPor);

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

                Respuesta = await this.ProcesoSimple(() => _estadoProyectoRepository.Actualizar(EstadoProyecto, conexion, transaction), "");

                if (!Respuesta.Satisfactorio)
                {
                    Respuesta.Status = StatusCodes.Status500InternalServerError;
                    return Respuesta;
                }

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new(false, "El usuario ya esta registrado", ex.Message, StatusCodes.Status400BadRequest);

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
            return Respuesta;
        }

    }
}
