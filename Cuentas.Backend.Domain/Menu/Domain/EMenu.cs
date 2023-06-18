using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Menu.Domain
{
    public class EMenu
    {
        public int Id { get; set; }
        public int Nivel { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public int MenuPadre_Id { get; set; }
        public string Icono { get; set; }
        public string ColorFondo { get; set; }
        public string ColorTexto { get; set; }
        public string ColorIcono { get; set; }
        public int Orden { get; set; }
        public bool Habilitado { get; set; }
        public DateTime Creado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? Modificado { get; set; }
        public string ModificadoPor { get; set; }
    }
}
