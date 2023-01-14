using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Token.DTO
{
    public class InToken
    {
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
    }
    public class InTokenValidator : AbstractValidator<InToken>
    {
        public InTokenValidator()
        {

            RuleFor(x => x.Usuario).NotNull().NotEmpty().MinimumLength(1);
            RuleFor(x => x.Contrasenia).NotNull().NotEmpty().MinimumLength(1);
        }

    }
}
