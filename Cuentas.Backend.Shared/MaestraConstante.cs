using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Shared
{
    public class MaestraConstante
    {


        public static readonly string MENSAJE_ERROR_GENERICO = "Ocurrio un error";

        public static readonly string MENSAJE_ERROR_GENERICO_CATH = "Ocurrio un error";

        public static readonly string MENSAJE_OPERACION_EXITOSA = "Operación existosa";




        public static readonly int USUARIO_SESSION_CODIGO = 1;

        public static readonly int EXPIRE_MINUTES = 480;

        public static readonly bool ESTADO_USUARIO_EXISTE = true;
        public static readonly string MENSAJE_ESTADO_USUARIO_EXISTE = "El usuario ya se encuentra registrado";


        public static readonly bool ESTADO_USUARIO_NO_EXISTE = false;
        public static readonly string MENSAJE_USUARIO_NO_EXISTE = "El usuario no se encuentra registrado";



    }
}
