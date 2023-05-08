using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuentas.Backend.Shared;
using Cuentas.Backend.Aplication.Comun;
using Microsoft.Extensions.Logging;
using Cuentas.Backend.Domain.AppAngular.DTO;
using System;
using System.Diagnostics;

namespace Cuentas.Backend.Aplication.Generar
{
    public class GenerarApp : BaseApp<GenerarApp>
    {
        public GenerarApp(ILogger<BaseApp<GenerarApp>> logger) : base(logger)
        {

        }
        public async Task<StatusSimpleResponse> GenerarEstructura(InGenerarApp datoApp)
        {

            StatusSimpleResponse Respuesta =new StatusSimpleResponse();

            if (!Respuesta.Satisfactorio)
                Respuesta.Status = StatusCodes.Status500InternalServerError;

            return Respuesta;
        }

    }
}
