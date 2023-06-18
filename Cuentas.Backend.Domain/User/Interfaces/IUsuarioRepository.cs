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
    public interface IUsuarioRepository
    {
        Task Registrar(UsuarioPortal usuario, SqlConnection conexion, SqlTransaction transaccion);

        Task Actualizar(UsuarioPortal usuario,int id, SqlConnection conexion, SqlTransaction transaccion);

        Task<UsuarioPortal> ValidarExistenciaDeNombreDeUsuario(string nombreUsuario, SqlConnection conexion, SqlTransaction transaccion);

        Task<UsuarioPortal> ValidarExistenciaDeNombreDeUsuarioSinTransaccion(string nombreUsuario);

        Task<Pagination<UsuarioPortal>> Listar(int page, int size, string? search, string? orderBy, string? orderDir);

    }
}
