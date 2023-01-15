using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Proyectos.Domain
{
    public class ProyectoDominio
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int EstadoProyecto_Id { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModifica { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
