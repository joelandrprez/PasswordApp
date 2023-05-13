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
        public string Ruta { get; set; }
        public int Tipo { get; set; }

    }
    public class InGenerarAppValidator : AbstractValidator<InGenerarApp>
    {
        public InGenerarAppValidator()
        {
            RuleFor(x => x.Ruta).NotNull().NotEmpty();
            RuleFor(x => x.Tipo).NotNull().NotEmpty();
        }

    }
}
