using Cuentas.Backend.Aplication.Comun;
using Cuentas.Backend.Domain.Cuentas.DTO;
using Cuentas.Backend.Domain.Cuentas.Interfaces;
using Cuentas.Backend.Shared;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public async Task<StatusSimpleResponse> Registrar(InCuenta cuenta) {
            DateTime FechaRegistro = DateTime.Now;

            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false,"");
            InCuentaValidator ValidacionCampos = new InCuentaValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(cuenta);

            if (!ResultadoValidacion.IsValid) {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errores = this.GetErrors(ResultadoValidacion.Errors);
                return Respuesta;
            }
            cuenta.FechaModificacion = FechaRegistro;
            cuenta.FechaCreacion = FechaRegistro;


            Respuesta.Satisfactorio = true;
            return Respuesta;
        
        }
        public async Task<StatusSimpleResponse> Actualizar(InCuenta cuenta)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse(false, "");

            InCuentaValidator ValidacionCampos = new InCuentaValidator();
            ValidationResult ResultadoValidacion = ValidacionCampos.Validate(cuenta);

            if (!ResultadoValidacion.IsValid)
            {
                Guid IdRespuestaError = new Guid();
                Respuesta.Id = IdRespuestaError;
                Respuesta.Titulo = "Los datos enviados no son válidos";
                Respuesta.Errores = this.GetErrors(ResultadoValidacion.Errors);
                return Respuesta;
            }

            cuenta.FechaModificacion = DateTime.Now;
            

            Respuesta.Satisfactorio = true;
            return Respuesta;

        }
    }
}
