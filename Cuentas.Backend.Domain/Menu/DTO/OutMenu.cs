using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Menu.DTO
{
    public class OutMenu
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }
        public string ColorFondo { get; set; }
        public string ColorTexto { get; set; }
        public int Orden { get; set; }
        public List<OutMenu> SubMenu { get; set; }
    }
}
