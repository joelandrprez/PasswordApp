using Cuentas.Backend.Aplication.Comun;
using Cuentas.Backend.Domain.Proyectos.Domain;
using Cuentas.Backend.Domain.Proyectos.DTO;
using Cuentas.Backend.Domain.Proyectos.Interfaces;
using Cuentas.Backend.Shared;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Aplication.Proyecto
{
    public class ProjectApp : BaseApp<ProjectApp>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectApp(ILogger<BaseApp<ProjectApp>> logger, IProjectRepository proyectRepository) : base(logger)
        {
            this._projectRepository = proyectRepository;
        }

        public async Task<StatusResponse<Pagination<Project>>> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Pagination<Project>> Respuesta = await this.ComplexProcess(() => _projectRepository.List(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Success)
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;

            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Save(InProject project, int idUserProcess)
        {
            DateTime FechaRegistro = DateTime.Now;

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, "");
            InProyectoValidator ValidacionCampos = new InProyectoValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(project);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid)
            {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Title = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }


           Project proyetoDominio = new();
           proyetoDominio.Descripcion = project.Descripcion;
           proyetoDominio.EstadoProyecto_Id = project.EstadoProyecto_Id;
           proyetoDominio.FechaCreacion = FechaProceso;
           proyetoDominio.UsuarioCrea = idUserProcess;
           proyetoDominio.FechaModificacion = FechaProceso;
           proyetoDominio.UsuarioModifica = idUserProcess;

           Respuesta = await this.SimpleProcess(() => _projectRepository.Save(proyetoDominio), "");

           if (!Respuesta.Success)
           {
               Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
               return Respuesta;
           }


            Respuesta.Title = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            Respuesta.Success = true;
            return Respuesta;

        }

        public async Task<StatusSimpleResponse> Update(InProject project, int id, int idUserProcess)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, "");

            DateTime FechaRegistro = DateTime.Now;

            InProyectoValidator ValidacionCampos = new InProyectoValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(project);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid)
            {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Title = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }



            Project proyetoDominio = new();
            proyetoDominio.Id = id;
            proyetoDominio.Descripcion = project.Descripcion;
            proyetoDominio.EstadoProyecto_Id = project.EstadoProyecto_Id;
            proyetoDominio.FechaModificacion = FechaProceso;
            proyetoDominio.UsuarioModifica = idUserProcess;


            Respuesta = await this.SimpleProcess(() => _projectRepository.Update(proyetoDominio), "");


            if (!Respuesta.Success)
            {
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }

            Respuesta.Title = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            Respuesta.Success = true;
            return Respuesta;

        }

    }

}
