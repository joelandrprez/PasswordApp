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
    public class ProyectoApp : BaseApp<ProyectoApp>
    {
        private readonly IProyectoRepository _proyectoRepository;

        public ProyectoApp(ILogger<BaseApp<ProyectoApp>> logger, IProyectoRepository proyectoRepository) : base(logger)
        {
            this._proyectoRepository = proyectoRepository;
        }

        public async Task<StatusResponse<Pagination<ProyectoDominio>>> Listar(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Pagination<ProyectoDominio>> Respuesta = await this.ProcesoComplejo(() => _proyectoRepository.Listar(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Satisfactorio)
                Respuesta.Status = StatusCodes.Status500InternalServerError;

            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Registrar(InProyecto proyecto, int idUsuarioProceso)
        {
            DateTime FechaRegistro = DateTime.Now;

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, "");
            InProyectoValidator ValidacionCampos = new InProyectoValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(proyecto);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid)
            {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errores = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.Status = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }


           ProyectoDominio proyetoDominio = new();
           proyetoDominio.Descripcion = proyecto.Descripcion;
           proyetoDominio.EstadoProyecto_Id = proyecto.EstadoProyecto_Id;
           proyetoDominio.FechaCreacion = FechaProceso;
           proyetoDominio.UsuarioCrea = idUsuarioProceso;
           proyetoDominio.FechaModificacion = FechaProceso;
           proyetoDominio.UsuarioModifica = idUsuarioProceso;

           Respuesta = await this.ProcesoSimple(() => _proyectoRepository.Registrar(proyetoDominio), "");

           if (!Respuesta.Satisfactorio)
           {
               Respuesta.Status = StatusCodes.Status500InternalServerError;
               return Respuesta;
           }


            Respuesta.Titulo = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            Respuesta.Satisfactorio = true;
            return Respuesta;

        }

        public async Task<StatusSimpleResponse> Actualizar(InProyecto proyecto, int id, int idUsuarioProceso)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, "");

            DateTime FechaRegistro = DateTime.Now;

            InProyectoValidator ValidacionCampos = new InProyectoValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(proyecto);
            DateTime FechaProceso = DateTime.Now;

            if (!ResultadoValidacion.IsValid)
            {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errores = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.Status = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }



            ProyectoDominio proyetoDominio = new();
            proyetoDominio.Id = id;
            proyetoDominio.Descripcion = proyecto.Descripcion;
            proyetoDominio.EstadoProyecto_Id = proyecto.EstadoProyecto_Id;
            proyetoDominio.FechaModificacion = FechaProceso;
            proyetoDominio.UsuarioModifica = idUsuarioProceso;


            Respuesta = await this.ProcesoSimple(() => _proyectoRepository.Actualizar(proyetoDominio), "");


            if (!Respuesta.Satisfactorio)
            {
                Respuesta.Status = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }

            Respuesta.Titulo = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            Respuesta.Satisfactorio = true;
            return Respuesta;

        }

    }

}
