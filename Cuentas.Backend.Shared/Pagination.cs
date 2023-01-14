using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Shared
{
    public class Pagination <T>
    {
        public int TotalGlobal { get; set; }
        public int TotalFiltered { get; set; }
        public string Previus { get; set; }
        public string Next { get; set; }
        public IEnumerable<T> Records { get; set; }
    }
}
