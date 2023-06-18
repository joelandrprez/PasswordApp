using Cuentas.Backend.Aplication.Comun;
using Cuentas.Backend.Domain.Proyectos.Domain;
using Cuentas.Backend.Domain.Proyectos.DTO;
using Cuentas.Backend.Domain.Proyectos.Interfaces;
using Cuentas.Backend.Shared;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;


namespace Cuentas.Backend.Aplication.Proyecto
{
    public class ProyectoApp : BaseApp<ProyectoApp>
    {
        private readonly IProyectoRepository _proyectoRepository;

        public ProyectoApp(ILogger<BaseApp<ProyectoApp>> logger, IProyectoRepository proyectoRepository) : base(logger)
        {
            this._proyectoRepository = proyectoRepository;
        }

        public async Task<StatusResponse<Paginacion<EProyecto>>> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            page = page ?? 1;
            size = size ?? 10;
            StatusResponse<Paginacion<EProyecto>> Respuesta = await this.ProcesoComplejo(() => _proyectoRepository.Listar(page.Value, size.Value, search, orderBy, orderDir));

            if (!Respuesta.Success)
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;

            return Respuesta;
        }

        public async Task<StatusSimpleResponse> Save(InProyecto proyecto, int creadoPor)
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
                Respuesta.Title = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }


           EProyecto proyetoDominio = new();
           proyetoDominio.Descripcion = proyecto.Descripcion;
           proyetoDominio.EstadoProyecto_Id = proyecto.EstadoProyecto_Id;
           proyetoDominio.FechaCreacion = FechaProceso;
           proyetoDominio.UsuarioCrea = creadoPor;
           proyetoDominio.FechaModificacion = FechaProceso;
           proyetoDominio.UsuarioModifica = creadoPor;

           Respuesta = await this.ProcesoSimple(() => _proyectoRepository.Registrar(proyetoDominio), "");

           if (!Respuesta.Success)
           {
               Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
               return Respuesta;
           }


            Respuesta.Title = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            Respuesta.Success = true;
            return Respuesta;

        }

        public async Task<StatusSimpleResponse> Update(InProyecto proyecto, int id, int creadoPor)
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
                Respuesta.Title = "Los datos enviados no son válidos";
                Respuesta.Errors = this.GetErrors(ResultadoValidacion.Errors);
                Respuesta.StatusCode = StatusCodes.Status500InternalServerError;
                return Respuesta;
            }



            EProyecto proyetoDominio = new();
            proyetoDominio.Id = id;
            proyetoDominio.Descripcion = proyecto.Descripcion;
            proyetoDominio.EstadoProyecto_Id = proyecto.EstadoProyecto_Id;
            proyetoDominio.FechaModificacion = FechaProceso;
            proyetoDominio.UsuarioModifica = creadoPor;


            Respuesta = await this.ProcesoSimple(() => _proyectoRepository.Actualizar(proyetoDominio), "");


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
