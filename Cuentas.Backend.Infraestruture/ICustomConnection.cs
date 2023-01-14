using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Infraestruture
{
    public interface ICustomConnection
    {
        Task<IDbConnection> BeginConnection();
        Task CloseConnection();
    }
}
