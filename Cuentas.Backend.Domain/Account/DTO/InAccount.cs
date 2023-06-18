using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Cuentas.DTO
{
    public class InAccount
    {
        public int? Id { get; set; } 
        public int TipoCuenta_Id { get; set; } 
        public string Sitio { get; set; } 
        public string Usuario { get; set; } 
        public string Password { get; set; } 
        public int Proyecto_Id { get; set; }
    }
    public class InCuentaValidator : AbstractValidator<InAccount>
    {
        public InCuentaValidator()
        {
            //RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.TipoCuenta_Id).NotEmpty();
            RuleFor(x => x.Sitio).NotNull().NotEmpty().MinimumLength(1);
            RuleFor(x => x.Usuario).NotNull().NotEmpty().MinimumLength(1);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(1);
            RuleFor(x => x.Proyecto_Id).NotNull().NotEmpty();
        }

    }
}


