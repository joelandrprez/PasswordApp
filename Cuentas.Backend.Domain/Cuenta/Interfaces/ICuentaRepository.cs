using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Domain.Cuentas.DTO;
using Cuentas.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Cuentas.Interfaces
{
    public interface ICuentaRepository
    {
        Task<Paginacion<ECuenta>> Listar(int page, int size, string? search, string? orderBy, string? orderDir);
        Task Registrar(ECuenta cuenta, SqlConnection conexion, SqlTransaction transaccion);
        Task Actualizar(ECuenta cuenta, SqlConnection conexion, SqlTransaction transaccion);
        Task<ECuenta> ObtenerPassword(int id);
    }
}
