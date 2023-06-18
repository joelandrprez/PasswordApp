using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.EstadoProyecto.DTO
{
    public class InProjectStatus
    {
        public bool Estado { get; set; }
        public string Descripcion { get; set; }
    }
    public class InEstadoProyectoValidator : AbstractValidator<InProjectStatus>
    {
        public InEstadoProyectoValidator()
        {
            RuleFor(x => x.Descripcion).NotNull().NotEmpty().MinimumLength(1);
        }

    }
}
