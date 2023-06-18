using Cuentas.Backend.Domain.Usuario.Domain;
using Cuentas.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Usuario.Interfaces
{
    public interface IUserRepository
    {
        Task Registrar(EUser usuario, SqlConnection conexion, SqlTransaction transaccion);

        Task Actualizar(EUser usuario,int id, SqlConnection conexion, SqlTransaction transaccion);

        Task<EUser> ValidarExistenciaDeNombreDeUsuario(string nombreUsuario, SqlConnection conexion, SqlTransaction transaccion);

        Task<EUser> ValidarExistenciaDeNombreDeUsuarioSinTransaccion(string nombreUsuario);

        Task<Pagination<EUser>> Listar(int page, int size, string? search, string? orderBy, string? orderDir);

    }
}
