using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.EstadoProyecto.DTO
{
    public class OutProjectStatus
    {
        public int Id { get; set; }
        public bool Estado { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModifica { get; set; }
    }
}
