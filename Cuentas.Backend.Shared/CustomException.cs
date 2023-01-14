using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Shared
{
    public class CustomException : Exception
    {
        public string Titulo { get; set; }
        public List<string> Errores { get; set; }
        public CustomException(string titulo, Exception exception) : base(titulo, exception)
        {
            this.Titulo = titulo;
        }

        public CustomException(string titulo, List<string> errores, Exception exception) : base(titulo, exception)
        {
            this.Titulo = titulo;
            this.Errores = errores;
        }
    }
}
