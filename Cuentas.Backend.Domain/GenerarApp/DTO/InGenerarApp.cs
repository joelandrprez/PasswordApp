using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Cuentas.Backend.Domain.AppAngular.DTO
{
    public class InGenerarApp
    {
        public string VersionAngular { get; set; }
        public string NombreProyecto { get; set; }
    }
    public class InProyectoValidator : AbstractValidator<InGenerarApp>
    {
        public InProyectoValidator()
        {
            //RuleFor(x => x.Id).NotNull().NotEmpty();

        }

    }
}
