using Cuentas.Backend.Domain.EstadoProyecto.Domain;
using Cuentas.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.EstadoProyecto.Interfaces
{
    public interface IEstadoProyectoRepository
    {
        Task Registrar(EEstadoProyecto estadoProyecto, SqlConnection conexion, SqlTransaction transaccion);

        Task Actualizar(EEstadoProyecto estadoProyecto, SqlConnection conexion, SqlTransaction transaccion);

        Task<Paginacion<EEstadoProyecto>> Listar(int page, int size, string? search, string? orderBy, string? orderDir);

    }
}
