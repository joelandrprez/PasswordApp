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
    public class ProjectStatusApp : BaseApp<ProjectStatusApp>
    {
        private readonly IProjectStatusRepository _statusProjectRepository;

        public ProjectStatusApp(ILogger<BaseApp<ProjectStatusApp>> logger, IConfiguration configuration, IProjectStatusRepository statusProjectRepository) : base(logger, configuration)
        {
            _statusProjectRepository = statusProjectRepository;
        }

        public async Task<StatusResponse<Pagination<EProjectStatus>>> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Pagination<EProjectStatus>> Respuesta = await this.ComplexProcess(() => _statusProjectRepository.List(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Success)
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;

            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Save(InProjectStatus statusProject, string createdBy)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse();
            InEstadoProyectoValidator validator = new InEstadoProyectoValidator();
            ValidationResult result = validator.Validate(statusProject);
            if (!result.IsValid)
            {
                Respuesta.Success = false;
                Respuesta.Title = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(result.Errors);
                Respuesta.StatusCode = StatusCodes.Status400BadRequest;
                return Respuesta;
            }

            DateTime FechaOperacion = DateTime.Now;
            EProjectStatus EstadoProyecto = new();
            EstadoProyecto.Estado = statusProject.Estado;
            EstadoProyecto.Descripcion = statusProject.Descripcion;
            EstadoProyecto.FechaModificacion = FechaOperacion;
            EstadoProyecto.UsuarioCrea = int.Parse(createdBy);
            EstadoProyecto.FechaCreacion = FechaOperacion;
            EstadoProyecto.UsuarioModifica = int.Parse(createdBy);

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

                Respuesta = await this.SimpleProcess(() => _statusProjectRepository.Save(EstadoProyecto, conexion, transaction), "");

                if (!Respuesta.Success)
                {
                    Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
                    return Respuesta;
                }

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new(false, MaestraConstante.MENSAJE_ERROR_GENERICO, ex.Message, StatusCodes.Status400BadRequest);

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
            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Update(InProjectStatus statusProject,int id, string creadoPor)
        {

            StatusSimpleResponse Respuesta = new StatusSimpleResponse();
            InEstadoProyectoValidator validator = new InEstadoProyectoValidator();
            ValidationResult result = validator.Validate(statusProject);
            if (!result.IsValid)
            {
                Respuesta.Success = false;
                Respuesta.Title = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(result.Errors);
                Respuesta.StatusCode = StatusCodes.Status400BadRequest;
                return Respuesta;
            }

            DateTime FechaOperacion = DateTime.Now;
            EProjectStatus EstadoProyecto = new();
            EstadoProyecto.Id = id;
            EstadoProyecto.Estado = statusProject.Estado;
            EstadoProyecto.Descripcion = statusProject.Descripcion;
            EstadoProyecto.FechaModificacion = FechaOperacion;
            EstadoProyecto.UsuarioCrea = int.Parse(creadoPor);
            EstadoProyecto.FechaCreacion = FechaOperacion;
            EstadoProyecto.UsuarioModifica = int.Parse(creadoPor);

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

                Respuesta = await this.SimpleProcess(() => _statusProjectRepository.Update(EstadoProyecto, conexion, transaction), "");

                if (!Respuesta.Success)
                {
                    Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
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


            Respuesta.Title = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            return Respuesta;
        }

    }
}

