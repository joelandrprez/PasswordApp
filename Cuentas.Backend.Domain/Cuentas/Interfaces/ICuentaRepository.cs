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
        Task<Pagination<Cuenta>> Listar(int page, int size, string? search, string? orderBy, string? orderDir);
        Task Registrar(Cuenta cuenta, SqlConnection conexion, SqlTransaction transaccion);
        Task Actualizar(Cuenta cuenta, SqlConnection conexion, SqlTransaction transaccion);
        Task<Cuenta> GetPassword(int id);
    }
}
