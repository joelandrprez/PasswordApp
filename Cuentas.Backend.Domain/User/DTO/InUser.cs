using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Usuario.DTO
{
    public class InUser
    {
        public int? Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
    }
    public class InUsuarioValidator : AbstractValidator<InUser>
    {
        public InUsuarioValidator()
        {

            RuleFor(x => x.NombreUsuario).NotNull().NotEmpty().MinimumLength(1).EmailAddress();
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(1);
        }

    }
}
