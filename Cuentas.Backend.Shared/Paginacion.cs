using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Shared
{
    public class Paginacion <T>
    {
        public int TotalGlobal { get; set; }
        public int TotalFiltrado { get; set; }
        public string Previo { get; set; }
        public string Seguiente { get; set; }
        public IEnumerable<T> Registros { get; set; }
    }
}
