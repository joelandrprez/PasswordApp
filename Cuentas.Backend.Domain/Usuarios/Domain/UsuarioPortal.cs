using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Usuario.Domain
{
    public class UsuarioPortal
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModificacion { get; set; }

    }
}
