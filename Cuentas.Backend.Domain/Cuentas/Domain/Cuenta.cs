using Cuentas.Backend.Domain.Proyectos.Domain;
using Cuentas.Backend.Domain.Tipocuentas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Cuentas.Domain
{
    public class Cuenta
    {
        public int? Id { get; set; }
        public int TipoCuenta_Id { get; set; }
        public string Sitio { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int Usuario_Id { get; set; }
        public int Proyecto_Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModificacion { get; set; }
        public TipoCuenta TipoCuentaDetalle { get; set; }
        public Proyectos.Domain.ProyectoDominio ProyectoDetalle { get; set; }

    }
}
